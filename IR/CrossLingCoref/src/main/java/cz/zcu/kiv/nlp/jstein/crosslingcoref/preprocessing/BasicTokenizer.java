package cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing;


public class BasicTokenizer implements Tokenizer {

    String splitRegex;

    public BasicTokenizer(String splitRegex) {
        this.splitRegex = splitRegex;
    }

    
    public String[] tokenize(String text) {
        return text.split(splitRegex);
    }
}
