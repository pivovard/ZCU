
__kernel void vector_add(__global const double* v1, __global const double* v2, __global double* r) {

	// Get the index of the current element to be processed
	int i = get_global_id(0);

	// Do the operation
	r[i] = v1[i] + v2[i];
}

__kernel void vector_sub(__global const double* v1, __global const double* v2, __global double* r) {

	// Get the index of the current element to be processed
	int i = get_global_id(0);

	// Do the operation
	r[i] = v1[i] - v2[i];
}

__kernel void vector_mul(__global const double* v1, __global const double* v2, __global double* r) {

	// Get the index of the current element to be processed
	int i = get_global_id(0);

	// Do the operation
	r[i] = v1[i] * v2[i];
}

__kernel void move_colony(__global const double* col, __global const double* imp, __global double* U, __global double* res) {
	int i = get_global_id(0);
	res[i] = col[i] + ((imp[i] - col[i]) * U[i]);
}




__kernel void sumGPU(__global const double* input, __global double* partialSums, __local double* localSums)
{
	uint local_id = get_local_id(0);
	uint group_size = get_local_size(0);

	// Copy from global to local memory
	localSums[local_id] = input[get_global_id(0)];

	// Loop for computing localSums : divide WorkGroup into 2 parts
	for (uint stride = group_size / 2; stride > 0; stride /= 2)
	{
		// Waiting for each 2x2 addition into given workgroup
		barrier(CLK_LOCAL_MEM_FENCE);

		// Add elements 2 by 2 between local_id and local_id + stride
		if (local_id < stride)
			localSums[local_id] += localSums[local_id + stride];
	}

	// Write result into partialSums[nWorkGroups]
	if (local_id == 0)
		partialSums[get_group_id(0)] = localSums[0];
}