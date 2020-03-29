

function [theta] = trainLinearReg (X, y, options)

	theta = train(getLinearRegression(), X, y, options);

end
