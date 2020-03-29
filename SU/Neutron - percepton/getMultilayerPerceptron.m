function [perceptron] = getMultilayerPerceptron

	perceptron.cost = @nnCost;
	perceptron.predict = @nnPredict;
	perceptron.opt = @gradientDescent;
	perceptron.unitCounts = [400, 25, 10];
	perceptron.options.iters = 1000;
	perceptron.options.alpha = 0.2;

end
