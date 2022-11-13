#include "ICA_opencl2.h"

bool ICA_opencl2::_INIT = false;
cl_context ICA_opencl2::context;
cl_program ICA_opencl2::program;
cl_command_queue ICA_opencl2::queue;
cl_kernel ICA_opencl2::kernel;
size_t ICA_opencl2::problemN = 0;

ICA_opencl2::ICA_opencl2(const solver::TSolver_Setup& setup) : ICA_smp(setup) {
	//init opencl
	if (!_INIT) {
		ICA_opencl2::init();
		_INIT = true;
	}
}

void ICA_opencl2::init()
{
	cl_int err = 0;
	cl_platform_id platform_id;
	cl_uint platforms_num;
	cl_device_id device_id;

	//get platforms
	err = clGetPlatformIDs(1, &platform_id, &platforms_num);
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: No suitable platform!");
	}

	
	//get device
	err = clGetDeviceIDs(platform_id, CL_DEVICE_TYPE_GPU, 1, &device_id, NULL);
	if (err != CL_SUCCESS)
	{
		throw std::exception("ERROR: No suitable device!");
	}


	cl_context_properties cprops[3] = { CL_CONTEXT_PLATFORM, (cl_context_properties)(platform_id), 0 };
	//get context
	context = clCreateContext(cprops, 1, &device_id, NULL, NULL, &err);
	if (!context || err != CL_SUCCESS)
	{
		throw std::exception("ERROR: Get context failed!");
	}

	queue = clCreateCommandQueue(context, device_id, 0, &err);
	if (!queue || err != CL_SUCCESS)
	{
		throw std::exception("ERROR: Create queue failed!");
	}

	const char* p = prog.c_str();
	program = clCreateProgramWithSource(context, 1, (const char**)&p, NULL, &err);
	if (!program || err != CL_SUCCESS)
	{
		throw std::exception("ERROR: Failed to load program!");
	}

	err = clBuildProgram(program, 0, NULL, NULL, NULL, NULL);
	if (err != CL_SUCCESS)
	{
		size_t len;
		char buffer[2048];

		printf("Error: Failed to build program executable!\n");
		clGetProgramBuildInfo(program, device_id, CL_PROGRAM_BUILD_LOG, sizeof(buffer), buffer, &len);
		printf("%s\n", buffer);

		throw std::exception("ERROR: Failed to build program!");
	}

	kernel = clCreateKernel(program, "move_colony", &err);
	if (!kernel || err != CL_SUCCESS)
	{
		throw std::exception("ERROR: Failed to load kernel!");
	}
}

void ICA_opencl2::finalize()
{
	cl_int err = 0;
	_INIT = false;
	err = clFlush(queue);
	err |= clFinish(queue);
	err |= clReleaseProgram(program);
	err |= clReleaseKernel(kernel);
	err |= clReleaseCommandQueue(queue);
	err |= clReleaseContext(context);

	if (err != CL_SUCCESS)
	{
		std::cout <<  err << std::endl;
		throw std::exception("ERROR: Failed to release object!");
	}
}

void ICA_opencl2::move_colony(Country& imp, Country& colony)
{
	//reandom vector - average 1 in 2 generations
	if (gen_double(0, 1) < 1.0 / (2 * setup.population_size)) {
		colony.vec = gen_vector(setup.problem_size, *setup.lower_bound, *setup.upper_bound);
		colony.fitness = calc_fitness(colony.vec);
		return;
	}

	if (setup.problem_size < cl_size) {
		ICA::move_colony(imp, colony);
		return;
	}

	cl_int err = 0;
	const size_t size = setup.problem_size;
	const size_t local_ws = 32; // Number of work-items per workgroup
	// shrRoundUp returns the smallest multiple of local_ws bigger than size
	const size_t global_ws = shrRoundUp(local_ws, size);

	std::vector<double> U = gen_vector(setup.problem_size, 0, 1); //U(0,1)
	std::vector<double> res(size);

	cl_mem v1 = clCreateBuffer(context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, NULL);
	cl_mem v2 = clCreateBuffer(context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, NULL);
	cl_mem u = clCreateBuffer(context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, NULL);
	cl_mem r = clCreateBuffer(context, CL_MEM_WRITE_ONLY, size * sizeof(double), NULL, NULL);

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to create buffer!");
	}

	err = clEnqueueWriteBuffer(queue, v1, CL_TRUE, 0, size * sizeof(double), colony.vec.data(), 0, NULL, NULL);
	err |= clEnqueueWriteBuffer(queue, v2, CL_TRUE, 0, size * sizeof(double), imp.vec.data(), 0, NULL, NULL);
	err |= clEnqueueWriteBuffer(queue, u, CL_TRUE, 0, size * sizeof(double), U.data(), 0, NULL, NULL);
	if (err != CL_SUCCESS)
	{
		clFinish(queue);
		clReleaseMemObject(v1);
		clReleaseMemObject(v2);
		clReleaseMemObject(u);
		clReleaseMemObject(r);
		throw std::exception("ERROR: Failed to write to the buffer!");
	}

	err = clSetKernelArg(kernel, 0, sizeof(cl_mem), &v1);
	err |= clSetKernelArg(kernel, 1, sizeof(cl_mem), &v2);
	err |= clSetKernelArg(kernel, 2, sizeof(cl_mem), &u);
	err |= clSetKernelArg(kernel, 3, sizeof(cl_mem), &r);

	if (err != CL_SUCCESS) {
		clFinish(queue);
		clReleaseMemObject(v1);
		clReleaseMemObject(v2);
		clReleaseMemObject(u);
		clReleaseMemObject(r);
		throw std::exception("ERROR: Failed to set arguments!");
	}

	err = clEnqueueNDRangeKernel(queue, kernel, 1, NULL, &global_ws, &local_ws, 0, NULL, NULL);
	if (err != CL_SUCCESS) {
		clFinish(queue);
		clReleaseMemObject(v1);
		clReleaseMemObject(v2);
		clReleaseMemObject(u);
		clReleaseMemObject(r);
		throw std::exception("ERROR: Failed to enqueue task!");
	}

	clFinish(queue);

	err = clEnqueueReadBuffer(queue, r, CL_TRUE, 0, size * sizeof(double), res.data(), 0, NULL, NULL);
	if (err != CL_SUCCESS) {
		clFinish(queue);
		clReleaseMemObject(v1);
		clReleaseMemObject(v2);
		clReleaseMemObject(u);
		clReleaseMemObject(r);
		throw std::exception("ERROR: Failed to read buffer!");
	}

	std::move(res.begin(), res.end(), colony.vec.begin());
	colony.fitness = calc_fitness(colony.vec);

	clReleaseMemObject(v1);
	clReleaseMemObject(v2);
	clReleaseMemObject(u);
	clReleaseMemObject(r);
}

size_t ICA_opencl2::shrRoundUp(size_t localWorkSize, size_t numItems) {
	size_t result = localWorkSize;
	while (result < numItems)
		result += localWorkSize;

	return result;
}