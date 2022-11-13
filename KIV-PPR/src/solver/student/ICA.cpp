#include"ICA.h"


thread_local std::mt19937_64 ICA::eng64;

ICA::ICA(const solver::TSolver_Setup& setup) : setup(setup)
{
	eng = std::mt19937(rd());
	eng64 = std::mt19937_64(rd());

	/*std::cout << "Problem size:\t" << setup.problem_size << std::endl;
	std::cout << "Population size:\t" << setup.population_size << std::endl;
	std::cout << "Upper bound:\t" << *setup.upper_bound << std::endl;
	std::cout << "Lower bound:\t" << *setup.lower_bound << std::endl;*/
}


void ICA::evolve()
{
	//move colonies
	for (auto& i : pop) {
		move_all_colonies(i);
	}

	if (pop.size() > 1) {
		//migrate colonies
		migrate_colonies();
	}
}

void ICA::move_all_colonies(Imperialist& imp)
{
	for (int i = 0; i < imp.colonies.size(); ++i) {
		move_colony(imp.imp, imp.colonies[i]);

		//if colonies cost function < than imperialists, switch
		if (imp.colonies[i].fitness < imp.imp.fitness) {
			auto tmp = imp.colonies[i];
			imp.colonies[i] = imp.imp;
			imp.colonies[i].imperialist = false;
			imp.imp = tmp;
			imp.imp.imperialist = true;
		}
	}
}

void ICA::move_colony(Country& imp, Country& colony)
{
	if (gen_double(0, 1) > 1.0 / (2*setup.population_size)) {
		//x ~ U(0,beta*d)
		//x ~ U(-gama,gama)
		std::vector<double> U = gen_vector(setup.problem_size, 0, 1); //U(0,1)
		//V=imp-col
		std::vector<double> V = vector_sub(imp.vec, colony.vec);
		//V=V*U
		V = vector_mul(V, U);
		//Xnew=Xold+V*U
		colony.vec = vector_add(colony.vec, V);
	}
	//reandom vector - average 1 in 2 generations
	else {
		colony.vec = gen_vector(setup.problem_size, *setup.lower_bound, *setup.upper_bound);
	}

	//count new fitness
	colony.fitness = calc_fitness(colony.vec);
}

void ICA::migrate_colonies()
{
	//count total fitness
	for (auto& i : pop) {
		double tc = calc_fitness_imp(i);
		i.total_fitness = tc;
	}

	const auto& imp_max = std::max_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.total_fitness < b.total_fitness; }); //worst
	const auto& imp_min = std::min_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.total_fitness < b.total_fitness; }); //best

	//imperium extinction
	if (imp_max->colonies.size() < 1) {
		imp_min->colonies.push_back(imp_max->imp);

		//copy imp to tmp
		tbb::concurrent_vector<Imperialist> tmp(pop);

		//copy tmp to imp without imperialist i
		_int64 d = std::distance(pop.begin(), imp_max);
		pop = tbb::concurrent_vector<Imperialist>(tmp.size() - 1);
		std::move(tmp.begin(), tmp.begin() + d, pop.begin());
		std::move(tmp.begin() + d + 1, tmp.end(), pop.begin() + d);
		
		return;
	}

	const auto& col_max = std::max_element(imp_max->colonies.begin(), imp_max->colonies.end(), [](Country a, Country b) { return a.fitness < b.fitness; }); //worst colony

	//add worst colony to the best imp
	imp_min->colonies.push_back(*col_max);

	//copy colonies to tmp
	tbb::concurrent_vector<Country> tmp(imp_max->colonies);

	//copy tmp to imp without imperialist i
	_int64 d = std::distance(imp_max->colonies.begin(), col_max);
	imp_max->colonies = tbb::concurrent_vector<Country>(tmp.size() - 1);
	std::move(tmp.begin(), tmp.begin() + d, imp_max->colonies.begin());
	std::move(tmp.begin() + d + 1, tmp.end(), imp_max->colonies.begin() + d);
	
}

void ICA::migrate_colonies_old()
{
	//double max_tc = DBL_MIN;
	double max_tc = get_max();
	double sum_tc = 0.0;

	//count total fitness
	for (auto& i : pop) {
		double tc = calc_fitness_imp(i);
		i.total_fitness = tc;
		sum_tc += tc;
		//if (tc > max_tc) max_tc = tc;
	}

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
	for (int i = 0; i < pop.size(); ++i) {
		for (int j = pop[i].colonies.size() - 1; j > -1; --j) {
			std::vector<double> R;
			R = gen_vector(P.size(), 0, 1);

			//D=P-R
			std::vector<double> D = vector_sub(P, R);
			
			auto it = std::min_element(D.begin(), D.end());
			_int64 max = std::distance(D.begin(), it);
			
			//migration
			if (max != i) {
				migration.push_back({ j, max, i });
			}
		}
	}

	do_migration(P, migration);
}

void ICA::do_migration(const std::vector<double>& P, const std::vector<std::vector<_int64>>& migration)
{
	//migration
	for (auto& vec : migration) {
		pop[vec[1]].colonies.push_back(pop[vec[2]].colonies[vec[0]]);

		//copy colonies to tmp
		tbb::concurrent_vector<Country> tmp(pop[vec[2]].colonies);

		//copy tmp to imp without imperialist i
		pop[vec[2]].colonies = tbb::concurrent_vector<Country>(tmp.size() - 1);
		std::move(tmp.begin(), tmp.begin() + vec[0], pop[vec[2]].colonies.begin());
		std::move(tmp.begin() + vec[0] + 1, tmp.end(), pop[vec[2]].colonies.begin() + vec[0]);
	}

	//imperialist losts power - must be serial
	for (int i = pop.size() - 1; i > -1; --i) {
		//if no colonies
		if (pop[i].colonies.size() == 0) {
			auto it = std::min_element(pop.begin(), pop.end(), [](Imperialist a, Imperialist b) { return a.total_fitness < b.total_fitness; });
			_int64 max = std::distance(pop.begin(), it);

			//extinction
			if (max != i) {
				pop[i].imp.imperialist = false;
				pop[max].colonies.push_back(pop[i].imp);
				//imp.erase(imp.begin() + i);

				//copy imp to tmp
				tbb::concurrent_vector<Imperialist> tmp(pop);
				//std::copy(pop.begin(), pop.end(), tmp.begin());

				//copy tmp to imp without imperialist i
				pop = tbb::concurrent_vector<Imperialist>(tmp.size() - 1);
				std::move(tmp.begin(), tmp.begin() + i, pop.begin());
				std::move(tmp.begin() + i + 1, tmp.end(), pop.begin() + i);

				--i; //size reduced by 1
			}
		}
	}
}

void ICA::do_migration(std::vector<double>& P, const tbb::concurrent_vector<std::vector<_int64>>& migration)
{
	//migration
	for (auto& vec : migration) {
		pop[vec[1]].colonies.push_back(pop[vec[2]].colonies[vec[0]]);

		//copy colonies to tmp
		tbb::concurrent_vector<Country> tmp(pop[vec[2]].colonies);

		//copy tmp to imp without imperialist i
		pop[vec[2]].colonies = tbb::concurrent_vector<Country>(tmp.size() - 1);
		std::move(tmp.begin(), tmp.begin() + vec[0], pop[vec[2]].colonies.begin());
		std::move(tmp.begin() + vec[0] + 1, tmp.end(), pop[vec[2]].colonies.begin() + vec[0]);
	}

	//imperialist losts power - must be serial
	for (int i = 0; i < pop.size(); ++i) {
		//if no colonies
		if (pop[i].colonies.size() == 0) {
			std::vector<double> R;
			R = gen_vector(P.size(), 0, 1);

			//D=P-R
			std::vector<double> D = vector_sub(P, R);

			auto it = std::min_element(D.begin(), D.end());
			_int64 max = std::distance(D.begin(), it);

			//extinction
			if (max != i) {
				pop[i].imp.imperialist = false;
				pop[max].colonies.push_back(pop[i].imp);
				//pop.erase(imp.begin() + i);

				//copy imp to tmp
				tbb::concurrent_vector<Imperialist> tmp(pop);

				//copy tmp to imp without imperialist i
				pop = tbb::concurrent_vector<Imperialist>(tmp.size() - 1);
				std::move(tmp.begin(), tmp.begin() + i, pop.begin());
				std::move(tmp.begin() + i + 1, tmp.end(), pop.begin() + i);
			}
		}
	}
}

std::vector<double> ICA::vector_add(std::vector<double>& vec1, std::vector<double>& vec2) {
	std::vector<double> res(vec1.size());
	std::transform(vec1.begin(), vec1.end(), vec2.begin(), res.begin(), std::plus<double>());
	return res;
}

std::vector<double> ICA::vector_sub(std::vector<double>& vec1, std::vector<double>& vec2) {
	std::vector<double> res(vec1.size());
	std::transform(vec1.begin(), vec1.end(), vec2.begin(), res.begin(), std::minus<double>());
	return res;
}

std::vector<double> ICA::vector_mul(std::vector<double>& vec1, std::vector<double>& vec2)
{
	std::vector<double> res(vec1.size());
	std::transform(vec1.begin(), vec1.end(), vec2.begin(), res.begin(), std::multiplies<double>());
	return res;
}

double ICA::get_min()
{
	const auto& it = std::min_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.imp.fitness < b.imp.fitness; });
	return it->imp.fitness;
}

double ICA::get_max()
{
	const auto& it = std::max_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.imp.fitness < b.imp.fitness; });
	return it->imp.fitness;
}

void ICA::write_solution()
{
	const auto& it = std::min_element(pop.begin(), pop.end(), [](Imperialist& a, Imperialist& b) { return a.imp.fitness < b.imp.fitness; });
	std::copy(it->imp.vec.begin(), it->imp.vec.end(), setup.solution);
}

double ICA::gen_double(double lower_bound, double upper_bound)
{
	std::uniform_real_distribution<> distr(lower_bound, upper_bound);
	return distr(eng64);
}

std::vector<double> ICA::gen_vector(size_t size, double lower_bound, double upper_bound)
{
	std::uniform_real_distribution<> distr(lower_bound, upper_bound);

	std::vector<double> vec(size);

	for (int j = 0; j < size; ++j) {
		double val = distr(eng64);
		vec[j] = val;
	}

	return vec;
}

void ICA::gen_population()
{
	tbb::concurrent_vector<Country> pop_s;

	//generate start population
	for (int i = 0; i < setup.population_size; ++i) {
		Country c;
		c.vec = gen_vector(setup.problem_size, *setup.lower_bound, *setup.upper_bound);
		pop_s.emplace_back(c);
	}

	//calc fitness functions and sort
	for (auto& country : pop_s) {
		country.fitness = calc_fitness(country.vec);
	}
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
		pop.emplace_back(tmp);
	}
	

	//assign colonies - not match number of colonies by the formula
	std::uniform_real_distribution<> dist; //distrinution (0,1)

	for (int i = n_imp; i < setup.population_size; ++i) {
		double roll = dist(eng);
		double tmp = 0;

		for (int j = 0; j < n_imp; ++j) {
			tmp += prob[j];
			if (roll <= tmp) {
				pop[j].colonies.emplace_back(pop_s[i]);
				break;
			}
		}
	}
}

double ICA::calc_fitness(const std::vector<double>& vec)
{
	double res = setup.objective(setup.data, vec.data());
	return res;
}

void ICA::calc_fitness_all()
{
	/*for (auto& country : pop) {
		country.fitness = calc_fitness(country.vec);
	}*/
}

double ICA::calc_fitness_imp(const Imperialist& imp)
{
	//without colonies increase cost
	if (imp.colonies.size() == 0) return 2*imp.imp.fitness;

	double sum = 0;
	for (int i = 0; i < imp.colonies.size(); ++i) {
		sum += imp.colonies[i].fitness;
	}

	//total imp cost = imp cost + 1/col_size * mean(cost of colonies)
	return imp.imp.fitness + sum / (2*imp.colonies.size());
}

double ICA::calc_distance(const std::vector<double>& vec1, const std::vector<double>& vec2)
{
	double sum = 0.0;

	for (int i = 0; i < vec1.size(); ++i) {
		sum += std::pow(vec1[i] - vec2[i], 2);
	}

	return std::sqrt(sum);
}

void ICA::print_population()
{
	for (const auto& i : pop) {
		std::cout << "Imp: ";
		std::cout << i.imp.fitness << " - ";
		print_vector(i.imp.vec);
		std::cout << "colonies = " << i.colonies.size();
		std::cout << std::endl;

		for (const auto& col : i.colonies) {
			std::cout << "\tCol: ";
			std::cout << col.fitness << " - ";
			print_vector(col.vec);
		}
	}

	std::cout << std::endl;
}



void ICA::print_vector(const std::vector<double>& vec)
{
	for (const auto& v : vec) {
		std::cout << v << " ";
	}
	std::cout << std::endl;
}
