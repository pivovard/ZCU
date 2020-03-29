function [J, grad] = logRegCost(X, y, theta, options)
  
if ~isfield(options, 'lambda')
	options.lambda = 0;
end

%TODO
%COSTFUNCTION Compute cost and gradient for logistic regression
%   J = logRegCost(theta, X, y) computes the cost of using theta as the
%   parameter for logistic regression and the gradient of the cost
%   w.r.t. to the parameters.

% Initialize some useful values
m = length(y); % number of training examples
lambda = options.lambda;

% You need to return the following variables correctly 
J = 0;
grad = zeros(size(theta));

% ====================== YOUR CODE HERE ======================
% Instructions: Compute the cost of a particular choice of theta.
%               You should set J to the cost.
%               Compute the partial derivatives and set grad to the partial
%               derivatives of the cost w.r.t. each parameter in theta
%
% Note: grad should have the same dimensions as theta
%

theta0 = theta;
theta0(1) = 0;

s = sigmoid(X*theta);
J = sum(-y'*log(s)-(1-y)'*log(1-s)) /m + lambda * sum(theta0.^2) /(2*m);
grad = X'*(s-y)/m + lambda*theta0/m;


% =============================================================

end
