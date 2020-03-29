function J = computeCost(X, y, theta)
%COMPUTECOST Compute cost for linear regression
%   J = COMPUTECOST(X, y, theta) computes the cost of using theta as the
%   parameter for linear regression to fit the data points in X and y

% ====================== YOUR CODE HERE ======================
% Instructions: Compute the cost of a particular choice of theta
%               You should set J to the cost.

suma = 0;

for(i=1:length(X))
  suma = suma + power(theta(1) + X(i,2)*theta(2) - y(i), 2);
end


% You need to return the following variables correctly 
J = suma/(2*length(y));







% =========================================================================

end
