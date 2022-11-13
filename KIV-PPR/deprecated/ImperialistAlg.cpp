#include"ImperialistAlg.h"

std::random_device rd2; // obtain a random number from hardware
std::mt19937 eng2(rd2()); // seed the generator


ImperialistAlg::ImperialistAlg(const solver::TSolver_Setup& setup) : setup(setup)
{
	std::cout << "Problem size:\t" << setup.problem_size << std::endl;
	std::cout << "Population size:\t" << setup.population_size << std::endl;
	std::cout << "Upper bound:\t" << *setup.upper_bound << std::endl;
	std::cout << "Lower bound:\t" << *setup.lower_bound << std::endl;
}


void ImperialistAlg::init()
{
	gen_population();
	print_population();
}

void ImperialistAlg::evolve()
{
	//move colonies
	for (auto& i : imp) {
		move_all_colonies(i);
	}

	//migrate colonies
	migrate_colonies();
}

void ImperialistAlg::move_all_colonies(Imperialist& imp)
{
	for (int i = 0; i < imp.colonies.size(); ++i) {
		move_colony(*imp.imp, *imp.colonies[i]);

		//calc new cost - already at move_colony
		//imp.colonies[i]->fitness = calc_fitness(imp.colonies[i]->vec);

		//if colonies cost function < than imperialists, switch
		if (imp.colonies[i]->fitness < imp.imp->fitness) {
			auto* tmp = imp.colonies[i];
			imp.colonies[i] = imp.imp;
			imp.colonies[i]->imperialist = false;
			imp.imp = tmp;
			imp.imp->imperialist = true;
		}
	}
}

void ImperialistAlg::move_colony(Country& imp, Country& colony)
{
	//x - U(0,beta*d) - nefunkcni, pravdepodobne smerovy vector je spatne
	//double d = calc_distance(imp.vec, colony.vec);
	//double U = gen_double(0, beta * d); //U(0,beta*d)
	std::vector<double> U = gen_vector(setup.problem_size, 0, 1); //U(0,1)

	//V=imp-col
	std::vector<double> V(setup.problem_size);
	std::transform(imp.vec.begin(), imp.vec.end(), colony.vec.begin(), V.begin(), std::minus<double>());

	//V=V*U
	//for (auto& v : V) v *= U;
	std::transform(V.begin(), V.end(), U.begin(), V.begin(), std::multiplies<double>());

	//Xnew=Xold+V*U
	std::transform(colony.vec.begin(), colony.vec.end(), V.begin(), colony.vec.begin(), std::plus<double>());

	//x - U(-gama,gama)
	//double O = gen_double(-gama, gama); //U(-gama, gama)

	//count new fitness
	colony.fitness = calc_fitness(colony.vec);
}

void ImperialistAlg::migrate_colonies()
{
	//double max_tc = DBL_MIN;
	double max_tc = get_max();
	double sum_tc = 0.0;

	//count total fitness
	for (auto& i : imp) {
		double tc = calc_fitness_imp(i);
		i.total_fitness = tc;
		sum_tc += tc;
		//if (tc > max_tc) max_tc = tc;
	}

	//count probability vector p=|NTC/sum NTC |
	std::vector<double> P;

	for (auto& i : imp) {
		double p = std::abs((i.total_fitness - max_tc) / sum_tc); //normalized - wrong  formula
		//double p = std::abs(i.total_fitness / sum_tc); // not normalized ok
		P.push_back(p);
	}

	//{colony, imp in, imp out}
	//{  j,      max,     i   }
	std::vector<std::vector<_int64>> migration;
	for (int i = 0; i < imp.size(); ++i) {
		//for (auto* col : imp[i].colonies) {
		//for (int j = 0; j < imp[i].colonies.size(); ++j) {
		for (int j = imp[i].colonies.size() -1; j > -1; --j) {
			std::vector<double> R;
			R = gen_vector(P.size(), 0, 1);

			//D=P-R
			std::vector<double> D(P.size());
			std::transform(P.begin(), P.end(), R.begin(), D.begin(), std::minus<double>());

			auto it = std::max_element(D.begin(), D.end());
			_int64 max = std::distance(D.begin(), it);

			//migration
			if (max != i) {
				//imp[max].colonies.push_back(imp[i].colonies[j]);
				//imp[i].colonies.erase(imp[i].colonies.begin()+j);
				migration.push_back({ j, max, i });
			}
		}
	}

	//migration
	for (auto& vec : migration) {
		imp[vec[1]].colonies.push_back(imp[vec[2]].colonies[vec[0]]);
		//imp[vec[2]].colonies.erase(imp[vec[2]].colonies.begin()+vec[0]);
	}

	//imperialist losts power
	for (int i = 0; i < imp.size(); ++i) {
		//if no colonies
		if (imp[i].colonies.size() == 0) {
			std::vector<double> R;
			R = gen_vector(P.size(), 0, 1);

			//D=P-R
			std::vector<double> D(P.size());
			std::transform(P.begin(), P.end(), R.begin(), D.begin(), std::minus<double>());

			auto it = std::max_element(D.begin(), D.end());
			_int64 max = std::distance(D.begin(), it);

			//extinction
			if (max != i) {
				imp[i].imp->imperialist = false;
				imp[max].colonies.push_back(imp[i].imp);
				imp.erase(imp.begin()+i);
			}
		}
	}
}

double ImperialistAlg::get_min()
{
	//auto& it = std::min(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	const auto& it = std::min_element(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	return it._Ptr->fitness;
}

double ImperialistAlg::get_max()
{
	//auto& it = std::min(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	const auto& it = std::max_element(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	return it._Ptr->fitness;
}

void ImperialistAlg::write_solution()
{
	//auto& it = std::min(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	const auto& it = std::min_element(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	std::copy(it._Ptr->vec.begin(), it._Ptr->vec.end(), setup.solution);
}

double ImperialistAlg::gen_double(double lower_bound, double upper_bound)
{
	std::uniform_real_distribution<> distr(lower_bound, upper_bound);
	return distr(eng2);
}

std::vector<double> ImperialistAlg::gen_vector(size_t size, double lower_bound, double upper_bound)
{
	std::uniform_real_distribution<> distr(lower_bound, upper_bound);

	std::vector<double> vec;

	for (int j = 0; j < size; ++j) {
		double val = distr(eng2);
		vec.push_back(val);
	}

	return vec;
}

void ImperialistAlg::gen_population()
{
	//generate start population
	for (int i = 0; i < setup.population_size; ++i) {
		Country c;
		c.vec = gen_vector(setup.problem_size, *setup.lower_bound, *setup.upper_bound);
		pop.push_back(c);
	}

	//calc fitness functions and sort
	calc_fitness_all();
	std::sort(pop.begin(), pop.end(), [](Country a, Country b) { return a.fitness < b.fitness; });
	
	//define imperialists
	size_t n_imp = start_imp;
	while (setup.population_size / n_imp >= max_colonies) {
		n_imp++;
	}

	double sum_C = 0; //sum of normalized fitnesses of imperialists
	double max_c = pop[setup.population_size - 1].fitness; //max of all countries OR maybe max of imperialists?

	for (int i = 0; i < n_imp; ++i) {
		Imperialist tmp;
		tmp.imp = &pop[i];
		imp.push_back(tmp);
		pop[i].imperialist = true;
		sum_C += pop[i].fitness - max_c;
	}
	
	std::vector<double> prob; //probability to get colony

	for (int i = 0; i < n_imp; ++i) {
		double p_n = std::abs((pop[i].fitness - max_c) / sum_C); //p=|norm.fitness/sum Cn|
		prob.push_back(p_n);
		std::cout << "Imp " << i+1 << " colonies " << std::round(p_n * (setup.population_size - n_imp)) << std::endl;
	}

	//assign colonies - not match number of colonies by the formula
	//tbb::paralel_for ??
	std::uniform_real_distribution<> dist; //distrinution (0,1)
	
	for (int i = n_imp; i < setup.population_size; ++i) {
		double roll = dist(eng2);
		double tmp = 0;

		for (int j = 0; j < n_imp; ++j) {
			tmp += prob[j];
			if (roll <= tmp) {
				imp[j].colonies.push_back(&pop[i]);
				break;
			}
		}
	}
}

double ImperialistAlg::calc_fitness(const std::vector<double>& vec)
{
	double res = setup.objective(setup.data, vec.data());
	return res;
}

void ImperialistAlg::calc_fitness_all()
{
	for (auto& country : pop) {
		country.fitness = calc_fitness(country.vec);
	}
}

double ImperialistAlg::calc_fitness_imp(const Imperialist& imp)
{
	//without colonies increase cost by 2*xi
	if (imp.colonies.size() == 0) return imp.imp->fitness + 2 * xi * imp.imp->fitness;

	double sum = 0;
	for (int i = 0; i < imp.colonies.size(); ++i) {
		sum += imp.colonies[i]->fitness;
	}
	
	//total imp cost = imp cost + xi*mean(cost of colonies)
	return imp.imp->fitness + xi*sum/imp.colonies.size();
}

double ImperialistAlg::calc_distance(const std::vector<double>& vec1, const std::vector<double>& vec2)
{
	double sum = 0.0;

	for (int i = 0; i < vec1.size(); ++i) {
		sum += std::pow(vec1[i] - vec2[i], 2);
	}

	return std::sqrt(sum);
}

void ImperialistAlg::print_population()
{
	for (const auto& i : imp) {
		std::cout << "Imp: ";
		std::cout << i.imp->fitness << " - ";
		print_vector(i.imp->vec);
		std::cout << "colonies = " << i.colonies.size();
		std::cout << std::endl;

		for (const auto& col : i.colonies) {
			std::cout << "\tCol: ";
				std::cout << col->fitness << " - ";
				print_vector(col->vec);
				std::cout << std::endl;
		}
	}

	std::cout << std::endl;
}

void ImperialistAlg::print_vector(const std::vector<double>& vec)
{
	for (const auto& v : vec) {
		std::cout << v << " ";
	}
}
