% J is the value of cost function
% grad is gradient of the cost function  with respect to parameters theta
% TODO
function [J, grad] = linRegCost(X, y, theta, options)

if ~isfield(options, 'lambda')
	options.lambda = 0;
end

% Initialize some useful values
m = length(y); % number of training examples
lambda = options.lambda;

theta0 = theta;
theta0(1) = 0;

J = sum((X*theta - y).^2) /(2*m) + lambda*sum(theta0.^2) /(2*m);
grad = X'*(X*theta-y)/m + lambda*theta0/m;

% =========================================================================

end
