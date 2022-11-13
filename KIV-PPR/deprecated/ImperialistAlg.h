#pragma once

#include <list>
#include <vector>
#include <iostream>
#include<random>

#include "../../common/iface/SolverIface.h"
#include "Country.h"

class ImperialistAlg
{
private:
	const solver::TSolver_Setup& setup;
	
	const double beta = 2.0;
	const double gama = std::_Pi / 4.0;
	const double xi = 0.1;

	const size_t start_imp = 3;
	const size_t max_colonies = 12;

public:
	std::vector<Country> pop;
	std::vector<Imperialist> imp;

	ImperialistAlg(const solver::TSolver_Setup& setup);
	~ImperialistAlg() = default;

	//to use in non parallel form
	void init();
	void evolve();

	void move_all_colonies(Imperialist& imp);
	void move_colony(Country& imp, Country& colony);
	void migrate_colonies();

	double get_min();
	double get_max();
	void write_solution();

	static double gen_double(double lower_bound, double upper_bound);
	static std::vector<double> gen_vector(size_t size, double lower_bound, double upper_bound);
	void gen_population();

	double calc_fitness(const std::vector<double>& vec);
	void calc_fitness_all();
	double calc_fitness_imp(const Imperialist& imp);

	double calc_distance(const std::vector<double>& vec1, const std::vector<double>& vec2);

	void print_population();
	void print_vector(const std::vector<double>& vec);
};