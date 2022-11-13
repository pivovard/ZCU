#pragma once
#include <vector>
#include<tbb/concurrent_vector.h>

struct Country
{
	std::vector<double> vec;
	double fitness = std::numeric_limits<double>::quiet_NaN();
	bool imperialist = false;
};

struct Imperialist
{
	Country imp;
	tbb::concurrent_vector<Country> colonies;
	double total_fitness = std::numeric_limits<double>::quiet_NaN();
};