clear all
close all
clc

data = [101 16; 105 18; 103 42; 98 23; 93 6];

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%Vlatní metoda
%S = cov(data);
% [V,L] = eig(S);
% V(:,[1,2,3,4,5,6,7,8,9])=V(:,[9,8,7,6,5,4,3,2,1]);
% 
% lambda = diag(L);
% lambda([1,2,3,4,5,6,7,8,9],:)=lambda([9,8,7,6,5,4,3,2,1],:);
% 
% result = data * V;
% 
% for i=1:length(lambda)
%     procenta(i) = lambda(i)/sum(lambda);
% end    

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%PCA funkce
%coeff = matice kterou se násobí pùvodní data
%latent = vlastní èísla
%explained = procenta
[coeff,score,latent,tsquared,explained,mu] = pca(data);
result2 = data*coeff;
coefforth = inv(diag(std(data)))* coeff;
B=data*coefforth;
% new_data = [result2(:,1), result2(:,2)];
% idx = kmeans(new_data, 6) ;