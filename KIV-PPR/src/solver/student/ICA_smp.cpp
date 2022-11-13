#include "ICA_smp.h"

ICA_smp::ICA_smp(const solver::TSolver_Setup& setup) : ICA(setup) {}

void ICA_smp::gen_population()
{
	tbb::concurrent_vector<Country> pop_s;

	//generate start population
	tbb::parallel_for(tbb::blocked_range<size_t>(0, setup.population_size), [&](const auto& r) {
		Country c;
		c.vec = gen_vector(setup.problem_size, *setup.lower_bound, *setup.upper_bound);
		pop_s.push_back(c);
	}, tbb::auto_partitioner());

	//calc fitness functions and sort
	tbb::parallel_for(size_t(0), setup.population_size, [&](size_t r) {
		pop_s[r].fitness = calc_fitness(pop_s[r].vec);
		}, tbb::auto_partitioner());
	std::sort(pop_s.begin(), pop_s.end(), [](Country a, Country b) { return a.fitness < b.fitness; });

	//define imperialists
	size_t n_imp = start_imp;
	while (setup.population_size / n_imp >= max_colonies) {
		n_imp++;
	}

	double sum_C = 0; //sum of normalized fitnesses of imperialists
	double max_c = pop_s[setup.population_size - 1].fitness; //max of all countries OR maybe max of imperialists?
	std::vector<double> prob(n_imp); //probability to get colony

	for (int i = 0; i < n_imp; ++i) {
		pop_s[i].imperialist = true;
		sum_C += pop_s[i].fitness - max_c;

		double p_n = std::abs((pop_s[i].fitness - max_c) / sum_C); //p=|norm.fitness/sum Cn|
		prob[i] = p_n;
		//std::cout << "Imp " << i + 1 << " colonies " << std::round(p_n * (setup.population_size - n_imp)) << std::endl;

		Imperialist tmp;
		tmp.imp = std::move(pop_s[i]);
		pop.push_back(tmp);
	}

	std::uniform_real_distribution<> dist; //distrinution (0,1)

	tbb::parallel_for(size_t(n_imp), setup.population_size, [&](size_t r) {
		double roll = dist(eng64);
		double tmp = 0;

		for (int j = 0; j < n_imp; ++j) {
			tmp += prob[j];
			if (roll <= tmp) {
				//std::cout << pop.size(); //dunno why this works (maybe console output is synchronized) - because colonies were std::vector, not parallel
				pop[j].colonies.push_back(pop_s[r]);
				break;
			}
		}
	}, tbb::auto_partitioner());
}

void ICA_smp::evolve()
{
	//move colonies
	tbb::parallel_for(size_t(0), pop.size(), [&](size_t r) {
		move_all_colonies(pop[r]);
	}, tbb::auto_partitioner());

	if (pop.size() > 1) {
		//migrate colonies
		migrate_colonies();
	}
}

void ICA_smp::move_all_colonies(Imperialist& imp)
{
	tbb::parallel_for(size_t(0), imp.colonies.size(), [&](size_t r) {
		move_colony(imp.imp, imp.colonies[r]);

		
	}, tbb::auto_partitioner());

	for (int r = 0; r < imp.colonies.size(); ++r) {
		//critical section, must be serial (mutex over imperialist would make it serial either)
		//if colonies cost function < than imperialists, switch
		if (imp.colonies[r].fitness < imp.imp.fitness) {
			auto tmp = imp.colonies[r];
			imp.colonies[r] = imp.imp;
			imp.colonies[r].imperialist = false;
			imp.imp = tmp;
			imp.imp.imperialist = true;
		}
	}
}

void ICA_smp::migrate_colonies_old()
{
	//double max_tc = DBL_MIN;
	double max_tc = get_max();
	double sum_tc = 0.0;
	tbb::mutex mutex;

	//count total fitness
	//may be parallel reduce
	tbb::parallel_for(size_t(0), pop.size(), [&](size_t r) {
		double tc = calc_fitness_imp(pop[r]);
		pop[r].total_fitness = tc;
		tbb::mutex::scoped_lock lock(mutex);
		sum_tc += tc;
	}, tbb::auto_partitioner());

	//count probability vector p=|NTC/sum NTC |
	size_t n_imp = pop.size();
	std::vector<double> P(n_imp);

	for (int i = 0; i < n_imp; ++i) {
		//double p = std::abs((i.total_fitness - max_tc) / sum_tc); //normalized
		double p = std::abs(pop[i].total_fitness / sum_tc); // not normalized
		P[i] = p;
	}

	//{colony, imp in, imp out}
	//{  j,      max,     i   }
	std::vector<std::vector<_int64>> migration;
	tbb::parallel_for(size_t(0), pop.size(), [&](size_t i) {
		for (int j = pop[i].colonies.size() - 1; j > -1; --j) {
			std::vector<double> R;
			R = gen_vector(P.size(), 0, 1);

			//D=P-R
			std::vector<double> D = vector_sub(P, R);

			auto it = std::min_element(D.begin(), D.end());
			_int64 max = std::distance(D.begin(), it);

			//migration
			if (max != i) {
				tbb::mutex::scoped_lock lock(mutex);
				migration.push_back({ j, max, _int64(i) });
			}
		}
	}, tbb::auto_partitioner());
	
	do_migration(P, migration);
}

void ICA_smp::calc_fitness_all()
{
	/*tbb::parallel_for(size_t(0), setup.population_size, [&](size_t r) {
		pop[r].fitness = calc_fitness(pop[r].vec);
	}, tbb::auto_partitioner());*/
}

double ICA_smp::calc_fitness_imp(const Imperialist& imp)
{
	//without colonies increase cost
	if (imp.colonies.size() == 0) return 2 * imp.imp.fitness;

	double sum = tbb::parallel_reduce(tbb::blocked_range<tbb::concurrent_vector<Country>::const_iterator>(imp.colonies.begin(), imp.colonies.end()), 0.0, [&](const auto& r, auto& init) -> double {
		//return std::accumulate(r.begin(), r.end(), init);
		double s = 0.0;
		for (auto itr = r.begin(); itr != r.end(); ++itr) {
			s += itr->fitness;
		}
		return s;
		},
		std::plus<double>());

	//total imp cost = imp cost + 1/col_size * mean(cost of colonies)
	return imp.imp.fitness + sum / (2 * imp.colonies.size());
}

double ICA_smp::get_min()
{
	double min = tbb::parallel_reduce(tbb::blocked_range<tbb::concurrent_vector<Imperialist>::iterator>(pop.begin(), pop.end()), 0.0, [&](const auto& r, auto& init) -> double {
		const auto& it = std::min_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.imp.fitness < b.imp.fitness; });
		return it->imp.fitness;
		},
		[](const double x, const double y) {return std::min(x, y); });
	return min;
}

double ICA_smp::get_max()
{
	double max = tbb::parallel_reduce(tbb::blocked_range<tbb::concurrent_vector<Imperialist>::iterator>(pop.begin(), pop.end()), 0.0, [&](const auto& r, auto& init) -> double {
		const auto& it = std::max_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.imp.fitness < b.imp.fitness; });
		return it->imp.fitness;
		},
		[](const double x, const double y) {return std::max(x, y); });
	return max;
}

