function [theta] = train(classifier, X, y, options)
%TRAINLINEARREG Trains linear regression given a dataset (X, y) and a
%regularization parameter lambda
%   [theta] = TRAINLINEARREG (X, y, lambda) trains linear regression using
%   the dataset (X, y) and regularization parameter lambda. Returns the
%   trained parameters theta.
%

% Initialize Theta
initial_theta = zeros(size(X, 2), 1); 

% Create "short hand" for the cost function to be minimized
costFunction = @(t) classifier.cost(X, y, t, options);

% Now, costFunction is a function that takes in only one argument
%options = optimset('MaxIter', 200, 'GradObj', 'on');
options.iters = 1000;

% Set Options
optOptions = optimset('GradObj', 'on', 'MaxIter', 400);

% Optimize
%[theta, J, exit_flag] = ...
%	fminunc(@(t)(costFunctionReg(t, X, y, lambda)), initial_theta, optOptions);

% Minimize using fmincg
% theta = fmincg(costFunction, initial_theta, options);
theta = gradientDescent(costFunction, initial_theta, options);

end
