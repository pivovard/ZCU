clear all
close all
clc

rozsah = 100;
muA = [2,3];
rho1 = 0.7;
s1 = 1;
s2 = 3;
sigmaA = [s1, sqrt(s1*s2)*rho1;sqrt(s1*s2)*rho1,s2];
rng default
A=mvnrnd(muA, sigmaA, rozsah);

%muB = [3,3];
%rho2 = -0.99; 
%sigmaB = [s1, sqrt(s1*s2)*rho2;sqrt(s1*s2)*rho2,s2];
%rng default
%B = mvnrnd(muA, sigmaA, rozsha);
%B = mvnrnd(muB, sigmaB, rozsha);

%Data = [A;B];
%S = [ones(size(A,1), 1); 2*ones(size(B,1),1)];

figure
hold on
plot(A(:,1),A(:,2),'o')
%plot(B(:,1),B(:,2),'xr')
%scatter(Data(:,1),Data(:,2),10,3,'filled');
grid on

MyScatter3(A(:,1),A(:,2))
%MyScatter3(Data(:,1),Data(:,2),3)

Data = A;
[n,m] = size(Data);
DataMean = mean(Data);
DataStd = std(Data);
B = (Data - repmat(DataMean, [n 1])) ./ repmat(DataStd, [n 1]);
B2 = zscore(Data);

MyScatter3(B(:,1),B(:,2))
MyScatter3(B2(:,1),B2(:,2))
[V,D] = eig(cov(B));
[coeff, score, latent, tsquared, explained, mu] = pca(B);

score2 = B * coeff;

Bopozit = (B * coeff) * coeff';
Dataopozit = ((B * coeff) * coeff') .* repmat(DataStd,[n 1]) + repmat(DataMean, [n 1])

var(score)
cumsum(var(score)) / sum(var(score))

figure
biplot(coeff(:,1:2),'scores',score(:,1:2));



