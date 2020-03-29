
function plotDecisionBoundaryMulti (x1, x2, yFunc, c)
	hold on;

	y = zeros(length(x1), length(x2));
	% Evaluate z = theta*x over the grid
	for i = 1:length(x1)
		for j = 1:length(x2)
			y(i,j) = yFunc(x1(i), x2(j));
		end
	end
	y = y'; % important to transpose z before calling contour

	% Plot z = 0
	% Notice you need to specify the range [0, 0]
	contour(x1, x2, y, [0, 0], 'LineWidth', 2, 'LineColor', c)
	hold off;
end
