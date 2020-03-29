function [probs] = checkProbabilities (X, model, numFeatures, numClasses)
	for i = 1 : numClasses
		probs(i) = sum(getFeatureClassProb(1 : numFeatures, i * ones(1, numFeatures), model));
	end
	probs
end
