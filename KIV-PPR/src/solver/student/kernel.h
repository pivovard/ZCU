#pragma once

#include<string>

const std::string prog = R"(
__kernel void vector_add(__global const double* v1, __global const double* v2, __global double* r) {
	int i = get_global_id(0);
	r[i] = v1[i] + v2[i];
}
__kernel void vector_sub(__global const double* v1, __global const double* v2, __global double* r) {
	int i = get_global_id(0);
	r[i] = v1[i] - v2[i];
}
__kernel void vector_mul(__global const double* v1, __global const double* v2, __global double* r) {
	int i = get_global_id(0);
	r[i] = v1[i] * v2[i];
}
__kernel void move_colony(__global const double* col, __global const double* imp, __global const double* U, __global double* res) {
	int i = get_global_id(0);
	res[i] = col[i] + ((imp[i] - col[i]) * U[i]);
})";