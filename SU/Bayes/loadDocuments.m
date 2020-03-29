function [X, y] = loadDocuments (file, vocabSize, numDocs)

mapping = load(file);
%X = spalloc(numDocs, vocabSize, 30 * numDocs);
X = zeros(numDocs, vocabSize);
i = mapping(:, 1) + mapping(:, 2) * numDocs;
X(i) = mapping(:, 3);

end
