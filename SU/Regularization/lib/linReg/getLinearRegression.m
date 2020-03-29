

function [linReg] = getLinearRegression ()

  linReg.cost  = @linRegCost;
  linReg.opt = @gradientDescent;
  linReg.predict = @linRegPredict;
  linReg.options.alpha = 0.01;
  linReg.options.num_iters = 400;
  linReg.options.lambda = 0;

end
