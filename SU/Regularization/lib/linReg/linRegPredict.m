function y = linRegPredict(X, theta)
%TODO linRegPredict Computes predicted value for linear regression
  y = (X - [0 mu]) ./ [1 sigma] * theta;
end
