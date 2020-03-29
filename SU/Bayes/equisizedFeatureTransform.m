
function [trans, model] = percentilleBasedFT (X, n, model)

m = size(X, 1);

if ~exist('model')
	tmp = quantile(X, linspace(0, 1, n + 1));
	model.upper = tmp(2 : end, :);
	model.upper(end, :) = model.upper(end, :) + .001;
	model.lower = tmp(1 : end - 1, :);
else
	n = size(model.upper, 1);
end

for i = 1: m
	for j = 1: size(X, 2)
		up = model.upper(:, j) > X(i, j);
		low = model.lower(:, j) <= X(i, j);
		trans(i, (j - 1) * n + 1: j * n) = up .* low;
	end
end


end
