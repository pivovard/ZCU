#pragma once

#include <iostream>
#include <fstream>
#include <vector>
#include <chrono>
#include <string>
#include "../../common/iface/SolverIface.h"


struct Stat
{
	long long time = 0;
	int generations = 0;
	std::vector<double> fitness;
	size_t population_size = 0;
	size_t problem_size = 0;
	short type = -1;
};

class Statistics
{
private:
	static std::vector<Stat> stats;
	static std::chrono::time_point<std::chrono::steady_clock> start_time;
	static short problem_n;

public:
	static void begin(const solver::TSolver_Setup& setup, short type);
	static void iteration(double cost);
	static std::vector<double> get_last_n(size_t n);
	static void end(int gen);
	static void clear();
	static void print_stat();
	static void export_stat();
};

