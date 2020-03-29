% costFunction has only one parameter: theta
% options is the structure of gradient descent settings
function [theta, thetaHistory, J_history] = gradientDescent(costFunction, initialTheta, options)

% Initialize some useful values
X = options.X;
y = options.y;
theta = initialTheta;


m = length(y);
J_history = [];
thetaHistory = [theta];
theta_tmp = zeros(size(X , 2), 1);

for i = 1:options.iters

	[J, grad] = costFunction(theta);
	
	theta = theta - options.alpha * grad;
  
  thetaHistory = [thetaHistory theta];
  J_history(i) = J;
  
  if(J < options.minCost || abs(thetaHistory'(length(thetaHistory)-1)(1) - theta) < options.minThetaDiff)
    break
  end

end

end