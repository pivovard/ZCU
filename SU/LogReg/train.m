function [theta, theta_history, J_history] = train (X, y)

	%  Set options for fminunc
	% options = optimset('GradObj', 'on', 'MaxIter', 400);
	[m, n] = size(X);
	options.iters = 2000;
	options.alpha = 4;
  options.minCost = 10^(-3);
  options.minThetaDiff = 10^(-3);
  
  
	% Initialize fitting parameters
	initial_theta = zeros(n, 1);
	%  Run fminunc to obtain the optimal theta
	%  This function will return theta and the cost 
	[theta, theta_history, J_history] = gradientDescent(@(t)(logRegCost(X, y, t)), initial_theta, options);

end
