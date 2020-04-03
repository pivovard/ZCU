package cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing;

import java.util.Map;


public interface Preprocessing {
    void index(String document);
    String getProcessedForm(String text);

    Map<String, Integer> getWordFrequencies();
}
