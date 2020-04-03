package cz.zcu.kiv.nlp.ir;

import java.util.Map;

/**
 * Created by tigi on 29.2.2016.
 */
public interface Preprocessing {
    void index(String document);
    String getProcessedForm(String text);

    Map<String, Integer> getWordFrequencies();
}
