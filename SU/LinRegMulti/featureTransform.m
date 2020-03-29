

function [X_trans] = featureTransform (X)
    textFeatureModel1 = dictionaryFT_train(squeeze(X(:, 1, :)));
    textFeatureModel2 = dictionaryFT_train(squeeze(X(:, 2, :)));
    X_trans_s1 = dictionaryFT_transform(squeeze(X(:, 1, :)), textFeatureModel1);
    X_trans_s2 = dictionaryFT_transform(squeeze(X(:, 2, :)), textFeatureModel2);
    for i = 3:10
      x_trans_i(i - 2, :) = str2double(squeeze(X(:, i, :)));
    end
	 x_trans_i(1:end -1, :) = featureNormalize(x_trans_i(1:end -1, :)')';
    X_trans = [reshape(X_trans_s1, size(X, 1), []) x_trans_i'];
end
