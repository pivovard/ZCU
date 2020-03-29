
function [p] = bayesPredict (X, model)
	[maximum, p] = max(bayesGetProbs(X, model), [], 2);
end
