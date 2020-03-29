% Computers performance score prediction
clc; clear all; close all;
 data = fileread('data_machines.txt');
 lines = char(strsplit(data, '\n'));
 features = char(32 * zeros(size(lines, 1), 10, 24));
 for(i = 1 : size(lines, 1))
    text = char(strsplit(lines(i, :), ','));
    features(i, 1:10, 1:size(text, 2)) = text;
    % fill empty cells with spaces
    features(i, 1:10, size(text, 2) + 1: end) = ' ';
 end
 X = featureTransform(features);
 y = X(:, end);
 X = X(:, 1:end - 1);
 m = length(y);
 % Add intercept term to X
X = [ones(m, 1) X];

[yPredict, yPerm] = crossValidation(X, y, 5);
fprintf('Test mean absolute error = %f\n', sum(abs(yPerm - yPredict)) / m);
theta = train(X, y);
yPredict = linRegPredict(X, theta);
fprintf('Train mean absolute error = %f\n', sum(abs(y - yPredict)) / m);
