%DRUHY UKOL
%Anezka Jachymova

clc
clear
close all

sample =1000; %pocet simulaci

f_join = @(x,y)((8/9 * (x) * (y)).*(1<x & x<y & y<2))
f_x = @(x)((x<= 2 & x >=1).*(-4/9 * x.*(x.^2-4)));



%vykresleni marginalni fce hustoty
figure
x = linspace(0,3,sample);% vektor se 100 cleny od -1 do 2
x2=-1:0.1:2; %jiny vektor

subplot(1,2,1)
y=f_x(x); %fcni hodnoty z f(x)
plot(x,y,'r','LineWidth',0.5)
title('marginalni fce hustoty pres body')
xlabel('x');
ylabel('y');

subplot(1,2,2)
ezplot(f_x, [0, 3]);
title('marginalni fce hustoty pomoci matlab fce')
xlabel('x');
ylabel('y');

%distribucni fce (spocitat, zadat jako fci, vykreslit)
F=@ (x)(integral(f_x,1,x)); %fce integralu
F(1);F(2);

for i=1:length(x)
    Fx(i)=F(x(i)); %vektor F(xi) s hodnotou distribucni fce
end

figure
plot(x,Fx)
title('distribucni fce')
xlabel('x');
ylabel('y');

%%%generovani inverzni
%%nejprve generovat x(i)
u=rand(sample,1);%nahodne cislo mezi 0 a 1, rovnomerne rozdeleni
v=rand(sample,1);%nahodne cislo mezi 0 a 1, rovnomerne rozdeleni pro podminenou ppst

% figure
% hist(u); hist(rand(1000,1));
% title('generovani - pozorovani rovnomerneho rozdeleni')

for i=1:sample
    SF=@(x) (integral(f_x,1,x))-u(i); %nalezeni nahodneho cisla
    xx(i)=fzero(SF,[1,2]);%fce na nalezeni numericke hledani korenu v danem intervalu
    
    fc_y=@(y)(f_join(xx(i),y)/f_x(xx(i))); %podminena ppst fce hustoty f(y/x1)
    SFC=@(y) (integral(fc_y,1,y))-v(i);
    yy(i)=fzero(SFC,[xx(i),2]); 
    %0<y<1-x
end


figure
subplot(2,1,1)
hist(xx)
subplot(2,1,2)
ecdf(xx)
title('pozorovani generovani x')
strednihodnota_x=mean(xx) %kolem 1/4
rozptyl_x=var(xx)%kolem 3/80

figure
minimum=min(yy)
maximum=max(yy)
subplot(2,1,1)
hist(yy) %vypada stejne jako u x
subplot(2,1,2)
ecdf(yy)
title('pozorovani generovani y')
strednihodnota_y=mean(yy) %kolem 1/4
rozptyl_y=var(yy)%kolem 3/80

kovariance=cov(xx,yy) %vypada, ze sem se trefila nebo ne???

figure
plot(xx,yy,'bo')
hold on
axis([0,3,0,3])
plot(strednihodnota_x,strednihodnota_y,'r*','Markersize',8) %'Marker','*'
title('generovani')
xlabel('x');
ylabel('y');