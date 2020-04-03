package cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing;

public class NullStemmer implements Stemmer{
	
	public String stem(String input) {
		return input;
	}

}
