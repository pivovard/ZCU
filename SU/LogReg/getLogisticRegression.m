function [logReg] = getLogisticRegression ()

  logReg.cost  = @logRegCost;
  logReg.opt = @gradientDescent;
  logReg.predict = @logRegPredict;
  logReg.options.alpha = 0.1;
  logReg.options.num_iters = 400;

end
