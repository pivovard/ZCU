clc
clear
close all

%KIV/MSM
%2.uloha
%Generovani 2D nahodne veliciny

%David Pivovar
%A19N0073P

%funkce
%f(x,y)=8/9*x*y; {(x,y); 1<x<y<2}
f_join = @(x,y) (8/9 .* x .* y).*(1<x & x<y & y<2)

%vykresleni funkce
[x,y] = meshgrid(0:.1:3)
z=f_join(x,y)

figure
surf(x,y,z)
%mesh(x,y,z)
xlabel('x')
ylabel('y')
zlabel('f(x,y)')

max=max(max(z))

%generovani
%pocet prvku
N=1000
%pocet iteraci
it=20

%stredni hodnota
mu=[;;]
%variace
va=[;;]
%(x,y)
points=[]

for i = 1:it
  u=rand(N,3)
  u=[u(:,1:2).+1,u(:,3).*5]
  
  res = find(u(:,1)<u(:,2) & u(:,3) < f_join(u(:,1),u(:,2)))
  
  points=[points;u(res,1),u(res,2)]
  mu=[mu;i,mean(u(res,1)),mean(u(res,2))]
  va=[va;i,var(u(res,1)),var(u(res,2))]
endfor

%Variancni matice
cov(points)

text=['Vypoctene hodnoty:\n'...
      'EX = 1.3926\n'...
      'EY = 1.7185\n'...
      'VarX = \t0.0607\t0.0266\n'...
             '\t0.0266\t0.0467']
sprintf(text)


figure
plot(points(:,1),points(:,2), 'x')
xlabel('x')
ylabel('y')

figure
subplot(2,2,1)
plot(mu(:,1),mu(:,2))
title('EX')
xlabel('iteration')
ylabel('EX')

subplot(2,2,2)
plot(mu(:,1),mu(:,3))
title('EY')
xlabel('iteration')
ylabel('EY')

subplot(2,2,3)
plot(va(:,1),va(:,2))
title('varX')
xlabel('iteration')
ylabel('varX')

subplot(2,2,4)
plot(va(:,1),va(:,3))
title('varY')
xlabel('iteration')
ylabel('varY')