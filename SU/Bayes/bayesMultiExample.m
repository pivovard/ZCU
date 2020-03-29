
%% Initialization
clear ; close all; clc

%% Setup the parameters you will use for this part of the exercise
num_labels = 10;          % 10 labels, from 1 to 10   
                          % (note that we have mapped "0" to label 10)

%% =========== Part 1: Loading and Visualizing Data =============
%  We start the exercise by first loading and visualizing the dataset. 
%  You will be working with a dataset that contains handwritten digits.
%

% Load Training Data
fprintf('Loading and Visualizing Data ...\n')

load('data/data2.mat'); % training data stored in arrays X, y
m = size(X, 1);

% Randomly select 100 data points to display
rand_indices = randperm(m);
X = X(rand_indices, :);
y = y(rand_indices);
sel = X(1:100, :);

buckets = 10;
[X, model] = equidistantFeatureTransform(X, buckets);
%[X, transModel] = equisizedFeatureTransform(X, buckets);

trainMax = 0.8 * m;
X_train = X(1 : trainMax, :);
X_test = X(trainMax + 1: end, :);

yTrain = y(1 : trainMax);
yTest = y(trainMax + 1: end);


displayData(sel);

fprintf('Program paused. Press enter to continue.\n');
pause;

%% ============ Part 2: Vectorize Logistic Regression ============

fprintf('\nTraining One-vs-All Logistic Regression...\n')

model = trainBayes(X_train, yTrain, num_labels);

%checkProbabilities(X, model, 400 * buckets, num_labels);

fprintf('Program paused. Press enter to continue.\n');
pause;


%% ================ Part 3: Predict for One-Vs-All ================

pred = bayesPredict(X_train, model);
predTest = bayesPredict(X_test, model);

% too slow
%[predCross, yPerm] = crossValidation(@(X, y)(oneVsAll(getLogisticRegression(), X, y, num_labels)), @predictOneVsAll, X_train, yTrain, 500);

fprintf('\nTraining Set Accuracy: %f\n', mean(double(pred == yTrain)) * 100);
fprintf('\nTesting Set Accuracy: %f\n', mean(double(predTest == yTest)) * 100);
%fprintf('\nCross-validation Accuracy: %f\n', mean(double(predCross == yPerm)) * 100);

