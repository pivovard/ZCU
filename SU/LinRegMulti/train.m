%TODO
function [theta, theta_history, J_history] = train (X, y)
	fprintf('Running gradient descent ...\n');
	% Choose some alpha value
	options.alpha = 0.2;
	options.iters = 1400;
  options.minCost = 10^(-3);
  options.minThetaDiff = 10^(-3);
  options.X = X;
  options.y = y;

	% Init Theta and Run Gradient Descent 
	initTheta = zeros(size(X, 2), 1);
	[theta, theta_history, J_history] = gradientDescent(@(theta)(linRegCost(X, y, theta)), initTheta, options);
end
