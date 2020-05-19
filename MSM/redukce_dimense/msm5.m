close all
clear all
clc

pkg load statistics

rozsah=100
muA=[2,3]
rho1=0.7
s1=1
s2=3
sigmaA=[s1,sqrt(s1*s2)*rho1;sqrt(s1*s2)*rho1,s2]

muB=[3,3]
rho2=-0.99
sigmaB=[s1,sqrt(s1*s2)*rho2;sqrt(s1*s2)*rho2,s2]

%rng default
A=mvnrnd(muA,sigmaA,rozsah)
B=mvnrnd(muB,sigmaB,rozsah)

Data=[A;B]
S=[ones(size(A,1),1);2*ones(size(B,1),1)]

figure
hold on
plot(A(:,1),A(:,2),'o')
plot(B(:,1),B(:,2),'rx')
scatter(Data(:,1),Data(:,2),10,5,'filled')
grid on

MyScatter3(A(:,1),A(:,2))
%MyScatter3(Data(:,1),Data(:,2),S)

