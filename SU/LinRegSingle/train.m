
% This function should initialize and train the parameters as column vector and return it
% as a model. The model will be passed to predict function
function [theta, theta_history, J_history] = train(X, y)
  theta = [0,0];
  theta_history = [0,0];
  sum = [0,0];
  tmp = [0,0];
  alfa = 0.02;
  J_history = [];
  J_history(1) = computeCost(X,y,theta);
  

  for(it=1:1000)
  
  for(i=1:size(y))
    sum(1) = sum(1) + theta(1) + X(i,2)*theta(2) - y(i);
    sum(2) = sum(2) + (theta(1) + X(i,2)*theta(2) - y(i)) * X(i,2);
  end
  
  tmp(1) = theta(1) - alfa * sum(1) /length(y);
  tmp(2) = theta(2) - alfa * sum(2) /length(y);
  
  theta(1) = tmp(1);
  theta(2) = tmp(2);
  
  J_history(it+1) = computeCost(X,y,theta);
  
  sum = [0,0];
  theta_history = [theta_history;theta];
  
  end
  
  theta = transpose(theta)
  
end
