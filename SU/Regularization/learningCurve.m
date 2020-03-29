function [error_train, error_val] = ...
    learningCurve(X, y, Xval, yval, lambda, trainFunc)
%LEARNINGCURVE Generates the train and cross validation set errors needed 
%to plot a learning curve
%   [error_train, error_val] = ...
%       LEARNINGCURVE(X, y, Xval, yval, lambda) returns the train and
%       cross validation set errors for a learning curve. In particular, 
%       it returns two vectors of the same length - error_train and 
%       error_val. Then, error_train(i) contains the training error for
%       i examples (and similarly for error_val(i)).
%
%   In this function, you will compute the train and test errors for
%   dataset sizes from 1 up to m. In practice, when working with larger
%   datasets, you might want to do this in larger intervals.
%

% Number of training examples
m = size(X, 1);
n = size(Xval, 1);

% You need to return these values correctly
error_train = []; %zeros(m, 1);
error_val   = []; %zeros(m, 1);

options.lambda = lambda;
options.alpha = 0.001;

% ====================== YOUR CODE HERE ======================
% Instructions: Fill in this function to return training errors in 
%               error_train and the cross validation errors in error_val. 
%               i.e., error_train(i) and 
%               error_val(i) should give you the errors
%               obtained after training on i examples.
%

for(i=1:m)
  x1 = X(1:i,:)
  y1 = y(1:i)
  
  theta = trainFunc(x1, y1, options);
  error_train = [error_train linRegCost(x1, y1, theta, options)];
  error_val   = [error_val linRegCost(Xval, yval, theta, options)];
end



% -------------------------------------------------------------

% =========================================================================

end
