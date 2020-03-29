% Plots cost values with change of theta
% works  for 3-dimensional theta only
function plotThetaJ (X, y, optimal_theta, costFunction)

	var = 4;
	% Grid over which we will calculate J
	theta0_vals = linspace(optimal_theta(1) - var, optimal_theta(1) + var, 100);
	theta1_vals = linspace(optimal_theta(2) - var, optimal_theta(2) + var, 100);
	theta2_vals = linspace(optimal_theta(3) - var, optimal_theta(3) + var, 100);

	% initialize J_vals to a matrix of 0's
	J_vals1 = zeros(length(theta0_vals), length(theta1_vals));
	J_vals2 = zeros(length(theta0_vals), length(theta1_vals));
	J_vals3 = zeros(length(theta0_vals), length(theta1_vals));

	% Fill out J_vals
	for i = 1:length(theta0_vals)
   	for j = 1:length(theta1_vals)
	  		t1 = [theta0_vals(i); theta1_vals(j); optimal_theta(3)];
			t2 = [theta0_vals(i); optimal_theta(2); theta2_vals(j)];
			t3 = [optimal_theta(1); theta1_vals(i); theta2_vals(j)];
	  		J_vals1(i,j) = costFunction(X, y, t1);
			J_vals2(i,j) = costFunction(X, y, t2);
			J_vals3(i,j) = costFunction(X, y, t3);
    	end
	end
	
	J_vals1 = J_vals1';
	J_vals2 = J_vals2';
	J_vals3 = J_vals3';
	figure;
	grid on;
	subplot(1, 3, 1);
	grid on;
	contour(theta0_vals, theta1_vals, J_vals1);
	xlabel('\theta_0'); ylabel('\theta_1');
	hold on;
	plot(optimal_theta(1), optimal_theta(2), 'rx', 'MarkerSize', 10);
	
	subplot(1, 3, 2);
	grid on;
	contour(theta0_vals, theta2_vals, J_vals2);
	xlabel('\theta_0'); ylabel('\theta_2');
	hold on;
	plot(optimal_theta(1), optimal_theta(3), 'rx', 'MarkerSize', 10);
	
	subplot(1, 3, 3);
	grid on;
	contour(theta1_vals, theta2_vals, J_vals3);
	xlabel('\theta_1'); ylabel('\theta_2');
	hold on;
	plot(optimal_theta(2), optimal_theta(3), 'rx', 'MarkerSize', 10);

end
