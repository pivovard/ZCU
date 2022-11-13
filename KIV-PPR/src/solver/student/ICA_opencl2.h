#pragma once

#include "ICA_smp.h"
#include "kernel.h"

#define CL_USE_DEPRECATED_OPENCL_1_2_APIS
#include <CL/cl.h>

class ICA_opencl2 : public ICA_smp {
private:
	//init flag
	static bool _INIT;

	static cl_context context;
	static cl_program program;
	static cl_command_queue queue;
	static cl_kernel kernel;

	//limit to run vector calc on GPU
	size_t cl_size = 0;

	//init OpenCL - platform, device, context, command queue, program
	void init();

	//returns min NDRange size based on work group size and count
	size_t shrRoundUp(size_t localWorkSize, size_t numItems);

protected:
	//move colony towards to imperialist - calc on GPU
	virtual void move_colony(Country& imp, Country& colony) override final;

public:
	ICA_opencl2(const solver::TSolver_Setup& setup);
	~ICA_opencl2() = default;

	//cleanup OpenCL
	void finalize();
	static size_t problemN;
};