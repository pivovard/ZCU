
%% Initialization
clear ; close all; clc

%% Load Data
%  The first two columns contains the exam scores and the third column
%  contains the label.

data = load('data1.txt');
X = data(:, [1, 2]); y = data(:, 3);

%% ==================== Part 1: Plotting ====================
%  We start the exercise by first plotting the data to understand the 
%  the problem we are working with.

fprintf(['Plotting data with + indicating (y = 1) examples and o ' ...
         'indicating (y = 0) examples.\n']);

plotData(X, y);

%fprintf('\nProgram paused. Press enter to continue.\n');
%pause;


%% ============ Part 2: Compute Cost and Gradient ============
%  In this part of the exercise, you will implement the cost and gradient
%  for logistic regression. You neeed to complete the code in 
%  costFunction.m

%  Setup the data matrix appropriately, and add ones for the intercept term
[m, n] = size(X);

% Scale features and set them to zero mean
fprintf('Normalizing Features ...\n');

[Xnorm mu sigma] = featureNormalize(X);

% Add intercept term to x and X_test
X = [ones(m, 1) X];
Xnorm = [ones(m, 1) Xnorm];

% Initialize fitting parameters
initial_theta = zeros(n + 1, 1);

% Compute and display initial cost and gradient
[cost, grad] = logRegCost(Xnorm, y, initial_theta);

fprintf('Cost at initial theta (zeros): %f\n', cost);
fprintf('Gradient at initial theta (zeros): \n');
fprintf(' %f \n', grad);

%fprintf('\nProgram paused. Press enter to continue.\n');
%pause;


%% ============= Part 3: Optimizing =============
%  In this exercise, you will use a built-in function (fminunc) to find the
%  optimal parameters theta.

[theta, thetaHistory, J_history] = train(Xnorm, y);

% Print theta to screen
% fprintf('Cost at theta found by fminun: %f\n', cost);
fprintf('theta: \n');
fprintf(' %f \n', theta);

[cost, grad] = logRegCost(Xnorm, y, theta);

fprintf('Cost at theta: %f\n', cost);

% Plot Boundary
plotDecisionBoundary(theta, Xnorm, y);

% Put some labels 
hold on;
% Labels and Legend
xlabel('Exam 1 score')
ylabel('Exam 2 score')

% Specified in plot order
legend('Admitted', 'Not admitted')
hold off;


% Plot the convergence graph
figure;
plot(1:numel(J_history), J_history, '-b', 'LineWidth', 2);
xlabel('Number of iterations');
ylabel('Cost J');

plotThetaJ(Xnorm, y, theta, @logRegCost);
%fprintf('\nProgram paused. Press enter to continue.\n');
%pause;

%% ============== Part 4: Predict and Accuracies ==============
%  After learning the parameters, you'll like to use it to predict the outcomes
%  on unseen data. In this part, you will use the logistic regression model
%  to predict the probability that a student with score 45 on exam 1 and 
%  score 85 on exam 2 will be admitted.
%
%  Furthermore, you will compute the training and test set accuracies of 
%  our model.
%
%  Your task is to complete the code in predict.m

%TODO
%  Predict probability for a student with score 45 on exam 1 
%  and score 85 on exam 2 

norm = ([1, 45, 85] - [0 mu]) ./ [1 sigma];
class = logRegPredict(norm, theta);
prob = sigmoid(norm*theta) *100;
fprintf(['For a student with scores 45 and 85, we predict an admission %d ' ...
         'probability of %f\n\n'], class, prob);

% Compute accuracy on our training set
p = logRegPredict(Xnorm, theta);
[pt, yPerm] = crossValidation(@train, @logRegPredict, Xnorm, y, 5);

fprintf('Train Accuracy: %f\n', mean(double(p == y)) * 100);
fprintf('Test Accuracy: %f\n', mean(double(pt == yPerm)) * 100);


