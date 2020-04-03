package cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashSet;
import java.util.Set;

public class StopwordsReader {

	static String file = "stopwords.txt";
	
	public static Set<String> Read(){
		Set<String> stopwords = new HashSet<String>();
    	String line;
    	
    	try {
    		BufferedReader  reader = new BufferedReader(new FileReader(new File(file)));
			while((line = reader.readLine()) != null) {
				stopwords.add(line);
			}
			reader.close();
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    	
    	return stopwords;
	}
	
	public Set<String> Read(String file){
		Set<String> stopwords = new HashSet<String>();
    	String line;
    	
    	try {
    		BufferedReader  reader = new BufferedReader(new FileReader(new File(file)));
			while((line = reader.readLine()) != null) {
				stopwords.add(line);
			}
			reader.close();
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    	
    	return stopwords;
	}
	
}
