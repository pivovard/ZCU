
%% Initialization
clear ; close all; clc

%% Load Data
%  The first two columns contains the exam scores and the third column
%  contains the label.

data = load('data/data1.txt');
X = data(:, [1, 2]); y = data(:, 3) + 1;

%% ==================== Part 1: Plotting ====================
%  We start the exercise by first plotting the data to understand the 
%  the problem we are working with.

fprintf(['Plotting data with + indicating (y = 1) examples and o ' ...
         'indicating (y = 0) examples.\n']);

plotData(X, y);

fprintf('\nProgram paused. Press enter to continue.\n');
pause;


%% ============ Part 2: Compute Cost and Gradient ============
%  In this part of the exercise, you will implement the cost and gradient
%  for logistic regression. You neeed to complete the code in 
%  costFunction.m

%  Setup the data matrix appropriately, and add ones for the intercept term
[m, n] = size(X);


%% ============= Part 3: Optimizing =============
%  In this exercise, you will use a built-in function (fminunc) to find the
%  optimal parameters theta.

options.alpha = 1;
buckets = 9;
[X, transModel] = equisizedFeatureTransform(X, buckets);
%[X, transModel] = equidistantFeatureTransform(X, buckets);
model = trainBayes(X, y, 2, options);
checkProbabilities(X, model, 2 * buckets, 2);

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

%prob = bayesGetProbs([85 45], model);
%fprintf(['For a student with scores 45 and 85, we predict an admission ' ...
%         'probability of %f\n\n'], prob);

% Compute accuracy on our training set
p = bayesPredict(X, model);
[pt, yPerm] = crossValidation(@(X, y)(trainBayes(X, y, 2, options)), @bayesPredict, X, y, 5);

fprintf('Train Accuracy: %f\n', mean(double(p == y)) * 100);
fprintf('Test Accuracy: %f\n', mean(double(pt == yPerm)) * 100);


