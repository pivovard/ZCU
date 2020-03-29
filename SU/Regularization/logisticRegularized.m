
%% Initialization
clear ; close all; clc

%% add libs
addpath('./lib/linReg');
addpath('./lib/logReg');
addpath('./lib/visualize');
addpath('./lib');

%% Load Data
%  The first two columns contains the X values and the third column
%  contains the label (y).

data = load('data2.txt');
X = data(:, [1, 2]); y = data(:, 3);

plotData(X, y);

% Put some labels 
hold on;

% Labels and Legend
xlabel('Microchip Test 1')
ylabel('Microchip Test 2')

% Specified in plot order
legend('y = 1', 'y = 0')
hold off;


%% =========== Part 1: Regularized Logistic Regression ============
%  In this part, you are given a dataset with data points that are not
%  linearly separable. However, you would still like to use logistic 
%  regression to classify the data points. 
%
%  To do so, you introduce more features to use -- in particular, you add
%  polynomial features to our data matrix (similar to polynomial
%  regression).
%

% Add Polynomial Features

% Note that mapFeature also adds a column of ones for us, so the intercept
% term is handled
X = mapFeature(X(:,1), X(:,2));

% Initialize fitting parameters
initial_theta = zeros(size(X, 2), 1);

% Set regularization parameter lambda to 1
options.lambda = 1;

% Compute and display initial cost and gradient for regularized logistic
% regression
[cost, grad] = logRegCost(X, y, initial_theta, options);

fprintf('Cost at initial theta (zeros): %f\n', cost);

%fprintf('\nProgram paused. Press enter to continue.\n');
%pause;

%% ============= Part 2: Regularization and Accuracies =============
%  Optional Exercise:
%  In this part, you will get to try different values of lambda and 
%  see how regularization affects the decision coundart
%
%  Try the following values of lambda (0, 1, 10, 100).
%
%  How does the decision boundary change when you vary lambda? How does
%  the training set accuracy vary?
%

% Initialize fitting parameters
initial_theta = zeros(size(X, 2), 1);

% Set regularization parameter lambda to 1 (you should vary this)
options.lambda = 0;
options.alpha = 20;
options.iters = 10000;

% Set Options
optOptions = optimset('GradObj', 'on', 'MaxIter', 400);

% Optimize
[theta, J, exit_flag] = ...
	gradientDescent(@(t)(logRegCost(X, y, t, options)), initial_theta, options);

% use for better optimization	
%[theta, J, exit_flag] = ...
%	fminunc(@(t)(logRegCost(X, y, t, options)), initial_theta, optOptions);

% Plot Boundary
% Here is the grid range
u = linspace(-1, 1.5, 50);
v = linspace(-1, 1.5, 50);
plotDecisionBoundaryMulti(u, v, @(x1, x2)(mapFeature(x1, x2)*theta), 'r');
hold on;
title(sprintf('lambda = %g', options.lambda))

% Labels and Legend
xlabel('Microchip Test 1')
ylabel('Microchip Test 2')

%legend('y = 1', 'y = 0')
%hold on;

% Compute accuracy on our training set
p = logRegPredict(X, theta);
lr = getLogisticRegression();
trainFunc = @(X1, y1)(train(lr, X1, y1, options));
[pc, yPerm] = crossValidation(trainFunc, @logRegPredict, X, y, 5);

fprintf('Train Accuracy: %f\n', mean(double(p == y)) * 100);

fprintf('Test Accuracy: %f\n', mean(double(pc == yPerm)) * 100);


%0.1
options.lambda = 0.1;
[theta, J, exit_flag] = ...
	gradientDescent(@(t)(logRegCost(X, y, t, options)), initial_theta, options);
  
plotDecisionBoundaryMulti(u, v, @(x1, x2)(mapFeature(x1, x2)*theta), 'g');

0.01
options.lambda = 0.01;
[theta, J, exit_flag] = ...
	gradientDescent(@(t)(logRegCost(X, y, t, options)), initial_theta, options);
  
plotDecisionBoundaryMulti(u, v, @(x1, x2)(mapFeature(x1, x2)*theta), 'b');

%0.001
options.lambda = 0.001;
[theta, J, exit_flag] = ...
	gradientDescent(@(t)(logRegCost(X, y, t, options)), initial_theta, options);
  
plotDecisionBoundaryMulti(u, v, @(x1, x2)(mapFeature(x1, x2)*theta), 'y');

legend('y = 1', 'y = 0', '1', '0.1', '0.01', '0.001')
