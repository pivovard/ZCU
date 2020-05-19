clc
clear all
close all

f_join=@(x,y) (8/9.*x*y).*((x>1) & (y>x) & (y<2));
[X,Y]=meshgrid(0:0.1:2);
figure
surf(X,Y,f_join(X,Y))

Samples=1000;
x=[];y=[];
tic
while length(x)<Samples
    %u1 a u2 pøedstavují x a y generovat tak aby odpovídali intervalu
    %koef pøed u3 = dosadit do fuknce max hodnotu z intervalu
    u=[1+rand(1,1),1+rand(1,1),3.55*rand(1,1)];
    %upravit první podmínku
    if (u(1)<u(2)) && (u(3)<=f_join(u(1),u(2)))
        x=[x,u(1)];
        y=[y,u(2)];
    end
end    
EX = [mean(x), mean(y)]
varX = cov(x, y)
toc

figure
plot(x,y,'x')
axis([0,2,0,2])