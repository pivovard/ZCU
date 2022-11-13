#include "ICA_opencl.h"

bool ICA_opencl::_INIT = false;
std::unique_ptr<std::vector<cl::Platform>> ICA_opencl::platform;
std::unique_ptr<cl::Context> ICA_opencl::context;
std::unique_ptr<std::vector<cl::Device>> ICA_opencl::device;
std::unique_ptr<cl::Program> ICA_opencl::program;
std::unique_ptr<cl::CommandQueue> ICA_opencl::queue;
std::unique_ptr<cl::Kernel> ICA_opencl::kernel;

std::unique_ptr<cl::Buffer> ICA_opencl::v1;
std::unique_ptr<cl::Buffer> ICA_opencl::v2;
std::unique_ptr<cl::Buffer> ICA_opencl::u;
std::unique_ptr<cl::Buffer> ICA_opencl::r;

size_t ICA_opencl::problemN = 0;

ICA_opencl::ICA_opencl(const solver::TSolver_Setup& setup) : ICA_smp(setup) {
	//init opencl
	if (!_INIT) {
		ICA_opencl::init();
		_INIT = true;
	}

	cl_int err = 0;
	const size_t size = setup.problem_size;
	v1 = std::make_unique<cl::Buffer>(cl::Buffer(*context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, &err));
	v2 = std::make_unique<cl::Buffer>(cl::Buffer(*context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, &err));
	u = std::make_unique<cl::Buffer>(cl::Buffer(*context, CL_MEM_READ_ONLY, size * sizeof(double), NULL, &err));
	r = std::make_unique<cl::Buffer>(cl::Buffer(*context, CL_MEM_WRITE_ONLY, size * sizeof(double), NULL, &err));
}

void ICA_opencl::init()
{
	cl_int err = 0;
	
	//get platforms
	std::vector<cl::Platform> platformList;
	cl::Platform::get(&platformList);
	std::cout << "Platforms: " << platformList.size() << std::endl;

	if (platformList.size() < 1) {
		throw std::exception("ERROR: No suitable platform!");
	}

	platform = std::make_unique<std::vector<cl::Platform>>(platformList);

	//get vendor of first platform
	std::string platformVendor;
	for (int i = 0; i < platformList.size(); ++i) {
		platformList[i].getInfo((cl_platform_info)CL_PLATFORM_VENDOR, &platformVendor);
		std::cout << "Platform " << i << " is: " << platformVendor << std::endl;
	}

	cl_context_properties cprops[3] = { CL_CONTEXT_PLATFORM, (cl_context_properties)(platformList[0])(), 0 };
	//get context
	ICA_opencl::context = std::make_unique <cl::Context>(cl::Context(CL_DEVICE_TYPE_GPU, cprops, NULL, NULL, &err));
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Get context failed!");
	}

	//get devices
	std::vector<cl::Device> deviceList;
	deviceList = ICA_opencl::context->getInfo<CL_CONTEXT_DEVICES>();
	std::cout << "Devices: " << deviceList.size() << std::endl;

	if (deviceList.size() < 1) {
		throw std::exception("ERROR: No suitable device!");
	}

	//get device name
	std::string deviceName;
	deviceList[0].getInfo((cl_device_info)CL_DEVICE_NAME, &deviceName);
	std::cout << "Device name is: " << deviceName << std::endl;

	ICA_opencl::device = std::make_unique<std::vector<cl::Device>>(deviceList);

	//load program
	//std::ifstream file("kernel.cl");
	//std::string prog(std::istreambuf_iterator<char>(file), (std::istreambuf_iterator<char>()));
	cl::Program::Sources source(1, std::make_pair(prog.c_str(), prog.length() + 1));

	ICA_opencl::program = std::make_unique<cl::Program>(cl::Program(*ICA_opencl::context, source, &err));
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to load program!");
	}

	err = program->build(deviceList);
	if (err != CL_SUCCESS) {
		std::string buildInfo;
		program->getBuildInfo(deviceList[0],CL_PROGRAM_BUILD_LOG, &buildInfo);
		std::cout << buildInfo << std::endl;
		throw std::exception("ERROR: Failed to build program!");
	}

	ICA_opencl::queue = std::make_unique<cl::CommandQueue>(cl::CommandQueue(*ICA_opencl::context, deviceList[0], 0, &err));

	ICA_opencl::kernel = std::make_unique<cl::Kernel>(cl::Kernel(*program, "move_colony", &err));
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to load kernel!");
	}
}

void ICA_opencl::releaseBuffers()
{
	try
	{
		v1.reset();
		v2.reset();
		u.reset();
		r.reset();
	}
	catch (const std::exception&)
	{
		throw std::exception("ERROR: Release unsuccessfull.");
	}
}

void ICA_opencl::releaseAll()
{
	_INIT = false;

	try
	{
		queue.reset();
		kernel.reset();
		program.reset();
		context.reset();
		device.reset();
		//context.reset();
		platform.reset();
	}
	catch (const std::exception&)
	{
		throw std::exception("ERROR: Release unsuccessfull.");
	}
}

void ICA_opencl::evolve() {
	ICA::evolve();
}

void ICA_opencl::move_all_colonies(Imperialist& imp) {
	ICA::move_all_colonies(imp);
}

void ICA_opencl::move_colony(Country& imp, Country& colony)
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

	/*cl::Kernel kernel(*program, "move_colony", &err);
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to load kernel!");
	}*/

	std::vector<double> U = gen_vector(setup.problem_size, 0, 1); //U(0,1)
	std::vector<double> res(size);

	//cl::Buffer v1(*context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), colony.vec.data(), &err);
	//cl::Buffer v2(*context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), imp.vec.data(), &err);
	//cl::Buffer u(*context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), U.data(), &err);
	//cl::Buffer r(*context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), res.data(), &err);

	queue->enqueueWriteBuffer(*v1, CL_TRUE, 0, size * sizeof(double), colony.vec.data());
	queue->enqueueWriteBuffer(*v2, CL_TRUE, 0, size * sizeof(double), imp.vec.data());
	queue->enqueueWriteBuffer(*u, CL_TRUE, 0, size * sizeof(double), U.data());

	queue->finish();

	err = kernel->setArg(0, *v1);
	err |= kernel->setArg(1, *v2);
	err |= kernel->setArg(2, *u);
	err |= kernel->setArg(3, *r);

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to set arguments!");
	}

	cl::Event event;
	err = queue->enqueueNDRangeKernel(*kernel, cl::NullRange, cl::NDRange(global_ws), cl::NDRange(local_ws), NULL, &event);
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to enqueue task!");
	}

	event.wait();
	queue->finish();

	err = queue->enqueueReadBuffer(*r, CL_TRUE, 0, size * sizeof(double), res.data());
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to read buffer!");
	}

	queue->finish();

	std::copy(res.begin(), res.end(), colony.vec.begin());
	colony.fitness = calc_fitness(colony.vec);
}

double ICA_opencl::calc_fitness(const std::vector<double>& vec)
{
	return ICA::calc_fitness(vec);
	//for schwefel only - couldn't find where is set problem number at setup
	const double mShift = -4.0;
	double result = 0.0;
	for (size_t i = 0; i < setup.problem_size; i++) {
		const double tmp = vec[i] + mShift;
		result -= tmp * sin(sqrt(fabs(tmp)));
	}
	return result;
}


std::vector<double> ICA_opencl::vector_add(std::vector<double>& vec1, std::vector<double>& vec2)
{
	if (setup.problem_size < cl_size) return ICA::vector_add(vec1, vec2);
	else return vector_op("vector_add", vec1, vec2);
}

std::vector<double> ICA_opencl::vector_sub(std::vector<double>& vec1, std::vector<double>& vec2)
{
	if (setup.problem_size < cl_size) return ICA::vector_sub(vec1, vec2);
	else return vector_op("vector_sub", vec1, vec2);
}

std::vector<double> ICA_opencl::vector_mul(std::vector<double>& vec1, std::vector<double>& vec2)
{
	if (setup.problem_size < cl_size) return ICA::vector_mul(vec1, vec2);
	else return vector_op("vector_mul", vec1, vec2);
}

std::vector<double> ICA_opencl::vector_op(std::string op, std::vector<double>& vec1, std::vector<double>& vec2)
{
	cl_int err = 0;
	const size_t size = vec1.size();
	const size_t local_ws = 32; // Number of work-items per workgroup
	// shrRoundUp returns the smallest multiple of local_ws bigger than size
	const size_t global_ws = shrRoundUp(local_ws, size);

	cl::Kernel kernel(*program, op.c_str(), &err);
	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to load kernel!");
	}

	std::vector<double> res(size);

	cl::Buffer v1(*context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), vec1.data(), &err);
	cl::Buffer v2(*context, CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), vec2.data(), &err);
	cl::Buffer r(*context, CL_MEM_WRITE_ONLY | CL_MEM_COPY_HOST_PTR, size * sizeof(double), res.data(), &err);

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to create buffer!");
	}

	err = kernel.setArg(0, v1);
	err |= kernel.setArg(1, v2);
	err |= kernel.setArg(2, r);

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to set arguments!");
	}

	cl::Event event;
	err = queue->enqueueNDRangeKernel(kernel, cl::NullRange, cl::NDRange(global_ws), cl::NDRange(local_ws), NULL, &event);

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to enqueue task!");
	}

	event.wait();
	err = queue->enqueueReadBuffer(r, CL_TRUE, 0, size * sizeof(double), res.data());

	if (err != CL_SUCCESS) {
		throw std::exception("ERROR: Failed to read buffer!");
	}

	return res;
}

size_t ICA_opencl::shrRoundUp(size_t localWorkSize, size_t numItems) {
	size_t result = localWorkSize;
	while (result < numItems)
		result += localWorkSize;

	return result;
}