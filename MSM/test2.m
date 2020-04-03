clc

text=['Vypoctene hodnoty:\n'...
      'EX = 1.3926\n'...
      'EY = 1.7185\n'...
      'VarX = \t0.0607\t0.0266\n'...
             '\t0.0266\t0.0467']
sprintf(text)
             
             return

u=rand(10,3)
u=[u(:,1:2).+1,u(:,3).*5]
res=[]

res = find(u(:,1)<u(:,2) & u(:,3)<f_join(u(:,1),u(:,2)))

f_join = @(x,y) (8/9 .* x .* y).*(1<x & x<y & y<2)
f_join(u(:,1),u(:,2))

u(:,3)<f_join(u(:,1),u(:,2))

x = [1,2,3]
return
y=[;;]
y=[y;x]
y=[y;u]
y=[y;5,5,5]


asd=y(:,2)
