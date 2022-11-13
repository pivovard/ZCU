#pragma once

#include "ICA_smp.h"
#include "kernel.h"

#define CL_USE_DEPRECATED_OPENCL_1_2_APIS
#include <CL/cl.hpp>
#include <fstream>


class ICA_opencl : public ICA_smp {
private:
	//init flag
	static bool _INIT;

	static std::unique_ptr<std::vector<cl::Platform>> platform;
	static std::unique_ptr<cl::Context> context;
	static std::unique_ptr<std::vector<cl::Device>> device;
	static std::unique_ptr<cl::Program> program;
	static std::unique_ptr<cl::CommandQueue> queue;
	static std::unique_ptr<cl::Kernel> kernel;

	static std::unique_ptr<cl::Buffer> v1;
	static std::unique_ptr<cl::Buffer> v2;
	static std::unique_ptr<cl::Buffer> u;
	static std::unique_ptr<cl::Buffer> r;

	//limit to run vector calc on GPU
	size_t cl_size = 0;

	//init OpenCL - platform, device, context, command queue, program
	void init();

	//perform vector operation on GPU
	std::vector<double> vector_op(std::string op, std::vector<double>& vec1, std::vector<double>& vec2);
	//returns min NDRange size based on work group size and count
	size_t shrRoundUp(size_t localWorkSize, size_t numItems);

protected:
	//calc fitness of colony
	virtual double calc_fitness(const std::vector<double>& vec) override final;
	//move of all colonies of imperialist
	virtual void move_all_colonies(Imperialist& imp) override final;
	//move colony towards to imperialist - calc on GPU
	virtual void move_colony(Country& imp, Country& colony) override final;

public:
	ICA_opencl(const solver::TSolver_Setup& setup);
	~ICA_opencl() = default;

	//algorithm iteration
	virtual void evolve() override final;

	//cleanup OpenCL
	void releaseBuffers();
	void releaseAll();
	static size_t problemN;

	//add 2 vectors
	virtual std::vector<double> vector_add(std::vector<double>& vec1, std::vector<double>& vec2) override final;
	//sub 2 vectors
	virtual std::vector<double> vector_sub(std::vector<double>& vec1, std::vector<double>& vec2) override final;
	//multiplies 2 vectors
	virtual std::vector<double> vector_mul(std::vector<double>& vec1, std::vector<double>& vec2) override final;

};