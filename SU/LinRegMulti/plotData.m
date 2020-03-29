function plotData(X, y)
%PLOTDATA Plots the data points x and y into a new figure 
%   PLOTDATA(x,y) plots the data points and gives the figure axes labels of
%   population and profit.
figure; % open a new figure window
colors = ['r', 'g', 'b', 'm', 'c'];
hold on;
for i = 1:5
    xi = X(find(X(:, 2) == i), 1);
    yi = y(find(X(:, 2) == i));
    plot(xi, yi, 'r.', 'MarkerSize', 10, 'color', colors(i)); % Plot the data
end

xlabel('Size in squere meters'); % Set the x??axis label
ylabel('Price in $s');
legend('one room', 'two rooms', 'three rooms', 'four rooms', 'five rooms', 'location', 'northeastoutside');
grid on;


% ============================================================

end
