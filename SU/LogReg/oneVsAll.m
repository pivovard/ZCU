function [all_theta] = oneVsAll(classifier, X, y, num_labels)
%TODO
%ONEVSALL trains multiple logistic regression classifiers and returns all
%the classifiers in a matrix all_theta, where the i-th row of all_theta 
%corresponds to the classifier for label i
%   [all_theta] = ONEVSALL(classifier, X, y, num_labels) trains num_labels
%   classifiers and returns each of these classifiers
%   in a matrix all_theta, where the i-th row of all_theta corresponds 
%   to the classifier for label i
%	 classifier is the classifier structure initialized by getLogisticRegression() function

% Some useful variables
[m, n] = size(X);

% You need to return the following variables correctly 
%all_theta = zeros(num_labels, n + 1);
all_theta = [];

% Add ones to the X data matrix
X = [ones(m, 1) X];

options.iters = 400;
options.alpha = 0.1;
options.minCost = 10^(-3);
options.minThetaDiff = 10^(-3);

% ====================== YOUR CODE HERE ======================
% Instructions: You should complete the following code to train num_labels
%               logistic regression classifiers with regularization
%               parameter lambda. 
%
% Hint: theta(:) will return a column vector.
%
% Hint: You can use y == c to obtain a vector of 1's and 0's that tell use 
%       whether the ground truth is true/false for this class.

for(i = 1:num_labels)
     % Set Initial theta
     initial_theta = zeros(n + 1, 1);
     
     [theta, theta_history, J_history] = gradientDescent(@(t)(logRegCost(X, (y == i), t)), initial_theta, options);
     %theta = fmincg(@(t)(logRegCost(X, (y == i), t)), initial_theta, options);
     
     all_theta = [all_theta theta];
end











% =========================================================================


end
