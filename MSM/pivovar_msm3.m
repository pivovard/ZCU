clc
clear
close all

mu = [2,1]
sigma = [5,3;3,2]
n = 100

X = mvnrnd(mu,sigma,n)

[n,p] = size(X)
mu_hat=mean(X)
sigma_hat=(X'*X/n-mean(X)'*mean(X))*n/(n-1)

S=sqrtm(sigma)
S_hat=sqrtm(sigma_hat)
Z=((S_hat)\(X-repmat(mu_hat,n,1))')'

figure
subplot(1,2,1)
plot(X(:,1),X(:,2),'xb')
subplot(1,2,2)
plot(Z(:,1),Z(:,2),'or')
axis square

alpha =0.05

F_teor=chi2inv(1-alpha,p)
F_esti=finv(1-alpha,p,n-p)*p*(n-1)/(n-p)

t=linspace(0,2*pi,100)
zz_teor=[sqrt(F_teor)*cos(t)',sqrt(F_teor)*sin(t)']
zz_esti=[sqrt(F_esti)*cos(t)',sqrt(F_esti)*sin(t)']

xx_teor=zz_teor*S+repmat(mu,length(zz_teor),1)
xx_esti=zz_esti*S_hat+repmat(mu_hat,length(zz_esti),1)

figure
subplot(1,2,1)
plot(Z(:,1),Z(:,2),'or')
patch(zz_teor(:,1),zz_teor(:,2),'g','Facealpha',0.03)
patch(zz_esti(:,1),zz_esti(:,2),'b','Facealpha',0.03)
axis square
subplot(1,2,2)
plot(X(:,1),X(:,2),'xb')
patch(xx_teor(:,1),xx_teor(:,2),'g','Facealpha',0.03)
patch(xx_esti(:,1),xx_esti(:,2),'b','Facealpha',0.03)
