% J is the value of cost function
% grad is gradient of the cost function  with respect to parameters theta
% TODO
function [J, grad] = linRegCost(X, y, theta)

% Initialize some useful values
m = length(y); % number of training examples


J = sum((X*theta - y).^2) / (2*m);
grad = X'*(X*theta-y)/m;

% =========================================================================

end
