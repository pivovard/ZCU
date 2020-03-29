/**
 * Copyright (c) 2014, Michal Konkol
 * All rights reserved.
 */
package cz.zcu.kiv.nlp.ir.trec.preprocessing;

/**
 * @author Michal Konkol
 */
public class BasicTokenizer implements Tokenizer {

    String splitRegex;

    public BasicTokenizer(String splitRegex) {
        this.splitRegex = splitRegex;
    }


    public String[] tokenize(String text) {
        return text.split(splitRegex);
    }
}
