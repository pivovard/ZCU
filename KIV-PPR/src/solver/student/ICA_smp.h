#pragma once

#include"ICA.h"
#include<tbb/parallel_for.h>
#include<tbb/parallel_reduce.h>
#include<tbb/blocked_range.h>
#include<tbb/concurrent_vector.h>
#include<tbb/mutex.h>
#include<numeric>

#include "../../common/iface/SolverIface.h"
#include "Country.h"

#if defined(_MSC_VER) && defined(_Wp64)
// Workaround for overzealous compiler warnings in /Wp64 mode
#pragma warning (disable: 4267)
#endif /* _MSC_VER && _Wp64 */

class ICA_smp : public ICA {
private:

protected:
	//parallel - move of all colonies of imperialist
	virtual void move_all_colonies(Imperialist& imp) override;
	//OLD parallel - random migration of colonies based on probability
	virtual void migrate_colonies_old() override final;

	//parallel - calc fitness of all colonies
	virtual void calc_fitness_all() override final;
	//calc fitness of imperialist - imp cost = imp_cost = imp_cost + 1/col_size * mean(cost_of_colonies) - parallel sum
	virtual double calc_fitness_imp(const Imperialist& imp) override final;

public:
	ICA_smp(const solver::TSolver_Setup& setup);
	~ICA_smp() = default;

	//parallel - generates new population
	virtual void gen_population() override final;
	//algorithm iteration - parallel move colonies
	virtual void evolve() override;
	
	//parallel_reduce for colony with min fitness function
	virtual double get_min() override final;
	//parallel_reduce for colony with max fitness function
	virtual double get_max() override final;
};