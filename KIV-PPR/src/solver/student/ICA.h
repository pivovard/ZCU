#pragma once

#include <list>
#include <vector>
#include <iostream>
#include<random>
#include<tbb/concurrent_vector.h>

#include "../../common/iface/SolverIface.h"
#include "Country.h"


class ICA
{
private:

protected:
	const solver::TSolver_Setup& setup;

	tbb::concurrent_vector<Imperialist> pop;

	const double beta = 2.0;
	const double gama = std::_Pi / 4.0;
	const double xi = 0.5;

	//start number of imperialist
	const size_t start_imp = 3;
	//max colonies for 1 imp at generation
	const size_t max_colonies = 12;

	std::random_device rd; // obtain a random number from hardware
	std::mt19937 eng; // seed the generator
	static thread_local std::mt19937_64 eng64;
	
	//move of all colonies of imperialist
	virtual void move_all_colonies(Imperialist& imp);
	//migrates of weakest colony of weakest imp to strongest imp
	virtual void migrate_colonies();
	//OLD random migration of colonies based on probability
	virtual void migrate_colonies_old();

	//calc fitness of all colonies
	virtual void calc_fitness_all();
	//calc fitness of imperialist - imp cost = imp_cost = imp_cost + 1/col_size * mean(cost_of_colonies)
	virtual double calc_fitness_imp(const Imperialist& imp);
	//calc fitness of colony
	virtual double calc_fitness(const std::vector<double>& vec);

	//move colony towards to imperialist
	virtual void move_colony(Country& imp, Country& colony);
	//OLD migrate colonies based on given vector
	void do_migration(const std::vector<double>& P, const std::vector<std::vector<_int64>>& migration);
	//OLD migrate colonies based on given vector
	void do_migration(std::vector<double>& P, const tbb::concurrent_vector<std::vector<_int64>>& migration);

public:
	ICA(const solver::TSolver_Setup& setup);
	~ICA() = default;

	//generates new population
	virtual void gen_population();
	//algorithm iteration
	virtual void evolve();

	//add 2 vectors
	virtual std::vector<double> vector_add(std::vector<double>& vec1, std::vector<double>& vec2);
	//sub 2 vectors
	virtual std::vector<double> vector_sub(std::vector<double>& vec1, std::vector<double>& vec2);
	//multiplies 2 vectors
	virtual std::vector<double> vector_mul(std::vector<double>& vec1, std::vector<double>& vec2);

	//returns colony with min fitness function
	virtual double get_min();
	//returns colony with max fitness function
	virtual double get_max();
	
	//calc distance between 2 colonies
	double calc_distance(const std::vector<double>& vec1, const std::vector<double>& vec2);

	//writes solution to the setup
	void write_solution();

	//generates random double value
	double gen_double(double lower_bound, double upper_bound);
	//generates vector of random double values
	std::vector<double> gen_vector(size_t size, double lower_bound, double upper_bound);

	//prints vector of double values
	void print_vector(const std::vector<double>& vec);
	//prints whole current population
	void print_population();
};