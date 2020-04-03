/**
 * Copyright (c) 2014, Michal Konkol
 * All rights reserved.
 */
package cz.zcu.kiv.nlp.ir;

/**
 * @author Michal Konkol
 */
public class BasicTokenizer implements Tokenizer {

    String splitRegex;

    public BasicTokenizer(String splitRegex) {
        this.splitRegex = splitRegex;
    }

    @Override
    public String[] tokenize(String text) {
        return text.split(splitRegex);
    }
}
