function [X_norm, mu, sigma] = featureNormalize(X)
%FEATURENORMALIZE Normalizes the features in X 
%   FEATURENORMALIZE(X) returns a normalized version of X where
%   the mean value of each feature is 0 and the standard deviation
%   is 1. This is often a good preprocessing step to do when
%   working with learning algorithms.
%   mu is vector of mean value of all features
%   sigma is vector of standard deviation of all features


%TODO You need to set these values correctly
X_norm = X;
mu = zeros(1, size(X, 2));
sigma = zeros(1, size(X, 2));


mu = mean(X);
sigma = std(X);
%sigma = max(X) - min(X);

X_norm = (X - mu) ./ sigma;


% ============================================================

end