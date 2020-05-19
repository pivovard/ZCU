close all
clear all
clc

alpha = 0.05
mu = [1, -1.5]
sigma = [1, -1.5; -1.5, 3]
n = 100
X =mvnrnd(mu, sigma, n)

function VykresliElipsu(X, alpha)
[n,p] = size(X)
mu_hat = mean(X)
S_hat = sqrtm(cov(X))

barva = 'b'

F_esti = findv(1-alpha, p, n-p)*p*(n-1)/(n-p)
t = linspace(0,2*pi,100)
zz_esti = [sqrt(F_esti)*cos(t)',sqrt(F_esti)*sin(t)']
xx_esti = zz_esti *S_hat +repmat(mu_hat, length(zz_esti),1)

figure
hold on
plot(X(:,1), X(:,2), 'x', 'color', barva)
patch(xx_esti(:,1), xx_esti(:,2), barva, 'Facealpha', alpha, 'edgecolor', barva, 'linestyle', '--', 'linewidth', 1)
MahX = mahal(X,X)
X_out = X(MahX>F_esti, :)
plot(X_out(:,1), X_out(:,2), 'o', 'MarkerFaceColor', barva)
plot(mu_hat(1), mu_hat(2), 'h', 'MakerEdgeColor', barva, 'MakerFaceColor', barva, 'MarkerSize', 10)