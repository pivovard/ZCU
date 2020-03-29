function [yPredict, yPerm] = crossValidation (X, y, foldN)
	% Randomize data
	perm = randperm(length(y));
	XPerm = X(perm, :);
	yPerm = y(perm);
	yPredict = zeros(length(y), 1);
	for i = 1:ceil(length(y) / foldN)
		testMin = (i - 1) * foldN + 1;
		testMax = min(i * foldN, length(y));
		
		xTrain = [XPerm(1: testMin - 1, :) ; XPerm(min(testMax + 1, length(y)) : end, :)];
		xTest = XPerm(testMin: testMax, :);
		yTrain = [yPerm(1: testMin - 1) ; yPerm(min(testMax + 1, length(y)): end)];
		theta = train(xTrain, yTrain);
		yPredict(testMin : testMax) = linRegPredict(xTest, theta);
	end
end
