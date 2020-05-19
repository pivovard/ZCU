clc
clear all
close all

syms x
syms y
syms c

a = 1;  %1<y<x<2
b = 2;

expr = c*(x+2*y);
fmin = a;
fmax = b;
smin = a;
smax = x;
a = integral2(expr,fmin, fmax, smin, smax)
f = solve('a=1')
rd = double(f)