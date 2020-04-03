clc
clear all
close all

f_join=@(x,y) (3/4.*(x+y)).*((x<=1) &(x>=0) & (y<=2*x));
%fsurf(f_join,[0,2,0,2])
[X,Y]=meshgrid(0:0.05:2);
figure
surf(X,Y,f_join(X,Y))

NumberSample=1000;
tic
x=[];y=[];n=0;
while length(x)<NumberSample
    n=n+1;
    u=[rand(1,1),2*rand(1,1),2.25*rand(1,1)];
    if (u(2)<=2*u(1)) && (u(3)<=f_join(u(1),u(2)))
    x=[x,u(1)];
    y=[y,u(2)];
    end
end    
toc

figure
plot(x,y,'o')
axis([0,1,0,2])