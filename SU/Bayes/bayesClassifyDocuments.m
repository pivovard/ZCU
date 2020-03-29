
clc; clear all; close all;

num_labels = 20;
vocabSize = 61288;
y = load('./data/train.label');
X = loadDocuments('./data/train.data', vocabSize, length(y));
fprintf('Train data loaded...');

options.smoothing = 1;

model = trainBayes(X, y, num_labels, options);

checkProbabilities(X, model, vocabSize, num_labels);

fprintf('classifier trained...');

yTest = load('./data/test.label');
X_test = loadDocuments('./data/test.data', vocabSize, length(yTest));

pTest = bayesPredict(X_test, model);
fprintf('test data predicted');
pTrain = bayesPredict(X, model);

fprintf('Train Accuracy: %f\n', mean(double(pTrain == y)) * 100);
fprintf('Test Accuracy: %f\n', mean(double(pTest == yTest)) * 100);

