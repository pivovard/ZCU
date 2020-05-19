clc
clear
close all

mu = [1,-1.5];
sigma = [1,-1.5;-1.5,3];
n = 100;
alpha =0.05;
X = mvnrnd(mu,sigma,n);
[n,p] = size(X);
mu_hat=mean(X);

Z=((sqrtm((X'*X/n-mean(X)'*mean(X))*n/(n-1)))\(X-repmat(mu_hat,n,1))')';
F_esti=finv(1-alpha,p,n-p)*p*(n-1)/(n-p);
t=linspace(0,2*pi,100);

esti1=[sqrt(F_esti)*cos(t)',sqrt(F_esti)*sin(t)'];
esti2=esti1*sqrtm((X'*X/n-mean(X)'*mean(X))*n/(n-1))+repmat(mu_hat,length(esti1),1);

figure
plot(Z(:,1),Z(:,2),'xb')
patch(esti1(:,1),esti1(:,2),'g','Facealpha',0.01)
axis square

figure
plot(X(:,1),X(:,2),'xr')
patch(esti2(:,1),esti2(:,2),'g','Facealpha',0.01)
axis square