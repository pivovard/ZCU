#include "..\solver_smp.h"
#include "ICA.h"
#include "Statistics.h"
#include<numeric>

HRESULT solve_serial(solver::TSolver_Setup &setup, solver::TSolver_Progress &progress) {

	//return S_FALSE;

	Statistics::begin(setup, 0);
	ICA ica(setup);

	ica.gen_population();
	//ica.print_population();

	int i = 0;
	size_t n = 100;
	double eps = 0.000000001;
	for (i; i < setup.max_generations; ++i) {
		if (progress.cancelled) return S_FALSE;

		ica.evolve();

		double cost_n = ica.get_min();
		Statistics::iteration(cost_n);

		//end if convergence stopped
		if (i > n) {
			std::vector<double> vec = Statistics::get_last_n(n);
			double sum = std::accumulate(vec.begin(), vec.end(), 0.0);

			if (std::abs(sum / n - vec.back()) < eps) break;
		}
	}

	ica.write_solution();
	//ica.print_population();

	Statistics::end(i);
	//Statistics::print_stat();

	//system("pause");
	return S_OK;
}