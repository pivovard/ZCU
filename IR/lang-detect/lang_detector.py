import os
import pickle

import numpy as np
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import confusion_matrix
from sklearn.metrics import precision_score, recall_score, f1_score, accuracy_score, classification_report
from sklearn.model_selection import train_test_split
from sklearn.naive_bayes import MultinomialNB, GaussianNB
from sklearn.neural_network import MLPClassifier
from sklearn.pipeline import Pipeline
from sklearn.svm import LinearSVC
from sklearn.tree import DecisionTreeClassifier

from config import languages, data_path, col_names, model_path, lang2label, label2lang
from print_cm import print_cm, plot_cm


def train(max_examples_per_lang=1000):
    """
    Train model, evaluate model and save it

    :param max_examples_per_lang: maximum number of examples per language used for training
    """
    X, y = load_data(max_examples_per_lang=max_examples_per_lang)

    # TODO
    # split loaded data into training and testing parts with ratio 0.2
    # you can use train_test_split() function from sklearn.model_selection or write your own function
    ### START CODE HERE ### (≈ 1 line of code)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
	### END CODE HERE ###

    print("Training size:" + str(len(X_train)))
    print("Testing  size:" + str(len(X_test)))

    # TODO
    # define vectorizer and classifier, use character n-grams and lowercasing
    # for vectorizer you can use TfidfVectorizer or CountVectorizer from sklearn.feature_extraction.text
    # for classifier use LogisticRegresssion, SVM (LinearSVC), MultinomialNB, MLPClassifier (MultiLayerPerceptron),
    # DecisionTreeClassifier...
    # https://scikit-learn.org/stable/modules/classes.html#module-sklearn.naive_bayes
    ### START CODE HERE ### (≈ 2 lines of code)
    #vectorizer = TfidfVectorizer()
    vectorizer = CountVectorizer()
    #classifier = LogisticRegression(random_state=0, solver='lbfgs', multi_class='multinomial')
    #classifier = LinearSVC()
    classifier = MultinomialNB(alpha=0.5, class_prior=None, fit_prior=False)
    ### END CODE HERE ###

    pipeline = Pipeline([
        ('vectorizer', vectorizer),
        ('classifier', classifier)
    ])

    # train model
    print('Training model...')
    model = pipeline.fit(X_train, y_train)

    # save model
    print('Saving model...')
    save_model(model, os.path.join(model_path, 'model.bin'))
    print('Model saved')

    # evaluate model
    evaluate_model(model, X_test, y_test)
    
    print('++++++++++++++++++++++++++++++++++++++++++++++++++++++')
    print('Custom evaluation:\n')
    
    evaluate_model_custom(model, X_test, y_test)
     
    print('++++++++++++++++++++++++++++++++++++++++++++++++++++++')


def evaluate_model_custom(model, X_test, y_test):
    # predict classes for testing data
    y_pred = model.predict(X_test)

    # TODO compute accuracy, confusion matrix, precisions, recalls, f1_scores for each class, and micro and macro f1 score
    ### START CODE HERE ###
    cm = compute_confusion_matrix(y_test, y_pred)
    
    total_prediction = y_test.size
    total_correct_prediction = 0
    
    for i in range(total_prediction):
        if y_pred[i] == y_test[i]:
            total_correct_prediction += 1
    
    accuracy = total_correct_prediction / total_prediction
    precisions = []
    recalls = []
    f1_scores = []
    macro_f1 = 0
    micro_f1 = 0
    
    tp = []
    fn = []
    fp = []
    
    for i, lang in enumerate(languages):
        tp.append(0)
        fn.append(0)
        fp.append(0)
    
        for j in range(total_prediction):
            if y_test[j] == i and y_pred[j] == i:
                tp[i] += 1
            if y_test[j] == i and y_pred[j] != i:
                fn[i] += 1
            if y_test[j] != i and y_pred[j] == i:
                fp[i] += 1
        
        precisions.append(tp[i]/(tp[i]+fp[i]))
        recalls.append(tp[i]/(tp[i]+fn[i]))
        f1_scores.append(2*precisions[i]*recalls[i]/(precisions[i]+recalls[i]))
        
    macroP = sum(precisions)/len(languages)
    macroR = sum(recalls)/len(languages)
    macro_f1 = 2*macroP*macroR/(macroP+macroR)
    
    microP = sum(tp)/(sum(tp)+sum(fp))
    microR = sum(tp)/(sum(tp)+sum(fn))
    micro_f1 = 2*microP*microR/(microP+microR)
    
    print()
    
    print('Total predictions: %d' % (total_prediction))
    print('Total correct predictions: %d' % (total_correct_prediction))
    print('Accuracy: %2.4f' % (accuracy))
    print('f1 macro: %2.4f' % (macro_f1))
    print('f1 micro: %2.4f' % (micro_f1))
    
    print()
    
    for i, lang in enumerate(languages):
        print(languages[i] + '   %2.6f' % precisions[i] + '   %2.6f' % recalls[i] + '   %2.6f' % f1_scores[i])
    
    ### END CODE HERE ###

def compute_confusion_matrix(y_test, y_pred):
    # TODO  compute confusion matrix
    
    cm = np.zeros((len(languages), len(languages)), dtype=int)
    
    for i in range(y_test.size):
        cm[y_test[i]][y_pred[i]] += 1
    
    print_cm(cm, labels=languages)
    
    return cm


def test_model(model_name):
    """
    Tests given model

    :param model_name:  file name of the model
    """

    model = load_model(os.path.join(model_path, model_name))
    predict(model, "Praha je největší město v České republice")
    predict(model, "The Sports Gazette; is an Italian daily newspaper")
    predict(model, "La Gazzetta dello Sport The Sports Gazette is an Italian daily newspaper")
    predict(model, "La Gazzetta dello Sport; The Sports Gazette is an Italian daily newspaper dedicated to coverage of various sports. Founded in 1896, it is the most widely read daily newspaper of any kind in Italy.")
    predict(model, "Fantacalcio rovente alla trentaseiesima giornata di Serie A")
    predict(model, "Ganz gleich, mit wem man spricht, die Berichte der Menschen in Iran ähneln sich. Da ist etwa der Besitzer eines großen Restaurants. Er lebt mittlerweile vor allem von Touristen und den sehr wohlhabenden Bürgern.")
    predict(model, "La misiva, fechada el pasado 1 de mayo y a la que ha tenido acceso EL PAÍS, llega cargada de amenazas, más o menos veladas, de posibles represalias políticas y comerciales si Bruselas mantiene su intención de desarrollar proyectos europeos de armamento sin apenas contar con países terceros, ni siquiera con EE UU")


def predict(model, sentence, print_info=True):
    """
    Predict language for a given sentence with a given model.

    :param model: model used for predictions
    :param sentence: sentence to be predicted
    :param print_info: print predicted examples
    :return: predicted label
    """

    y_pred = model.predict([sentence])

    # TODO
    # convert predicted class to label (0-> cs, 2->en) use label2lang dictionary from config.py
    ### START CODE HERE ### (≈ 1 line of code)
    label = label2lang[y_pred[0]]
    ### END CODE HERE ###
    if print_info is True:
        print("=======" + label.upper() + "======= " + sentence)

    return label


def evaluate_model(model, X_test, y_test, print_stats=True, print_misclassified=True, plot_confusion=True):
    """
    Evaluates given model and return metrics

    :return: accuracy, macro_f1, micro_f1, precisions, recalls, f1_scores
    """

    # predict classes for testing data
    y_pred = model.predict(X_test)

    # all examples prinitng
    # for i, sentence in enumerate(X_test):
    #     print('predicted as:' + str(y_pred[i]) + " lang:" + label2lang[y_pred[i]] + "----Sentence:---" + sentence)

    # incorrect examples
    if print_misclassified is True:
        for i, sentence in enumerate(X_test):
            if y_pred[i] != y_test[i]:
                print('label:' + str(y_test[i]) + " lang:" + label2lang[y_test[i]] + ' pred as:' + str(y_pred[i])
                      + " lang:" + label2lang[y_pred[i]] + "----Sentence:---" + sentence)

    precisions = precision_score(y_test, y_pred, average=None)
    recalls = recall_score(y_test, y_pred, average=None)
    f1_scores = f1_score(y_test, y_pred, average=None)

    accuracy = accuracy_score(y_test, y_pred)
    macro_f1 = f1_score(y_test, y_pred, average='macro')
    micro_f1 = f1_score(y_test, y_pred, average='micro')

    # print stats
    if print_stats is True:
        cm = confusion_matrix(y_test, y_pred)

        print(30 * '-')
        print_cm(cm, labels=languages)

        print(30 * '-')
        # The support is the number of occurrences of each class in y_true.
        report = classification_report(y_test, y_pred, target_names=languages, digits=6)
        string = '----Average----' + '\n' \
                 + 'accuracy: %2.4f ' % (accuracy) + '\n' \
                 + 'f1 macro score: %2.4f ' % (macro_f1) + '\n' \
                 + 'f1 micro score: %2.4f ' % (micro_f1)

        print(report)
        print(15 * '-')
        print(string)

        if plot_confusion is True:
            plot_cm(cm, labels=languages)

    return accuracy, macro_f1, micro_f1, precisions, recalls, f1_scores


def load_data(max_examples_per_lang=1000):
    """
    Loads data

    :param max_examples_per_lang: max number of examples to be load per one language
    :return: X, y numpy arrays
    """

    # load examples into pandas dataframe
    df = pd.concat((pd.read_csv(os.path.join(data_path, lang + '.tsv'),
                                names=col_names, encoding='utf-8', sep='\t', nrows=max_examples_per_lang)
                    for lang in languages), ignore_index=True)

    # TODO
    # map text label to numbers  cs-> 0, en-> 1 ... use lang2label dictionary from config.py
    # and function map() from pandas
    ### START CODE HERE ### (≈ 1 line of code)
    df['label'] = df['label'].map(lang2label)
    ### END CODE HERE ###


    X = df['text'].apply(lambda x: np.str_(x))
    y = df['label']

    # return numpy array
    return X.values, y.values


def save_model(model, path):
    with open(path, 'wb') as f:
        pickle.dump(model, f, pickle.HIGHEST_PROTOCOL)

def load_model(path):
    with open(path, 'rb') as f:
        return pickle.load(f)