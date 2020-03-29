function [theta] = normalEqn(X, y, options)


if ~isfield(options, 'lambda')
	options.lambda = 0;
end

% ====================== YOUR CODE HERE ======================
lambda = options.lambda;

J0 = eye(size(X,2));
J0(1,1) = 0;

theta = pinv(X'*X + lambda*J0)*X'*y;


% ============================================================

end
