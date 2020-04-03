package cz.zcu.kiv.nlp.ir;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

public class PubecPreprocessing {

	static Stemmer stemmer = new CzechStemmerAgressive();

    public static void main(String[] args) {
    	
        String text = "";
        String line;
        		
        try {
        	
        	
    		BufferedReader  reader = new BufferedReader(new FileReader(new File("data/allText.txt")));
			while((line = reader.readLine()) != null) {
				text += line;
			}
			reader.close();
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        		
        		
        final String[] tokenize = split(text, " ");
        Arrays.sort(tokenize);
        System.out.println(Arrays.toString(tokenize));

        System.out.println();

        final ArrayList<String> documents = new ArrayList<String>();
        for (String document : split(text, "\n")) {
            documents.add(document);
        }

//        wordsStatistics(documents, " ", false, null, false, false);

        System.out.println();
        System.out.println("-------------------------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", false, null, false, false, false);

        System.out.println();
        System.out.println("------------LOWERCASE---------------");
        System.out.println();
        wordsStatistics(documents, "\\S+", false, null, false, false, true);

        System.out.println();
        System.out.println("----------STEM Light------------------");
        System.out.println();
        
        stemmer = new CzechStemmerLight();
        wordsStatistics(documents, "\\S+", true, null, false, false, true);

        System.out.println();
        System.out.println("----------STEM Agressive------------------");
        System.out.println();

        stemmer = new CzechStemmerAgressive();
        wordsStatistics(documents, "\\S+", true, null, false, false, true);

        System.out.println();
        System.out.println("-----------ACCENTS---------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", true, null, false, true, true);
        
        System.out.println();
        System.out.println("-----------STOPWORDS---------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", true, StopwordsReader.Read(), false, true, true);


        System.out.println();
        System.out.println("-----------REGEX---------------");
        System.out.println();
        
        wordsStatistics(documents, AdvancedTokenizer.defaultRegex, true, StopwordsReader.Read(), false, true, true);

    }



    public static void wordsStatistics(List<String> lines, String tokenizeRegex, boolean stemm, Set<String> stopwords, boolean removeAccentsBeforeStemming, boolean removeAccentsAfterStemming, boolean toLowercase) {
        
        Map<String, Integer> words = new HashMap<String, Integer>();
        long numberOfWords = 0;
        long numberOfChars = 0;
        long numberOfDocuments = 0;
        for (String line : lines) {
            numberOfDocuments++;

            if (toLowercase) {
                line = line.toLowerCase();
            }
            if (removeAccentsBeforeStemming) {
                line = AdvancedTokenizer.removeAccents(line);
            }
            for (String token : tokenize(line, tokenizeRegex)) {
            //for (String token : BasicTokenizer.tokenize(line, BasicTokenizer.defaultRegex)) {
            	
            	if(token.equals("")) continue;
            	
            	if(stopwords != null && stopwords.contains(token)) {
            		continue;
                }
            	
                if (stemm) {
                    token = stemmer.stem(token);
                }
                if (removeAccentsAfterStemming) {
                    token = AdvancedTokenizer.removeAccents(token);
                }
                
                if (!words.containsKey(token)) {
                	words.put(token, 0);
                }
                
                numberOfWords++;
                numberOfChars += token.length();
                words.put(token, words.get(token) + 1);
            }
        }
        	
        System.out.println("numberOfWords: " + numberOfWords);
        System.out.println("numberOfUniqueWords: " + words.size());
        System.out.println("numberOfDocuments: " + numberOfDocuments);
        System.out.println("average document char length: " + numberOfChars / (0.0 + numberOfDocuments));
        System.out.println("average word char length: " + numberOfChars / (0.0 + numberOfWords));

        for (String key : words.keySet()) {
			System.out.println(key + " : " + words.get(key) + ", ");
		}
    }

    private static String[] split(String line, String regex) {
        return line.split(regex);
    }

    private static String[] tokenize(String line, String regex) {
        return AdvancedTokenizer.tokenize(line, regex);
    }
	
}
