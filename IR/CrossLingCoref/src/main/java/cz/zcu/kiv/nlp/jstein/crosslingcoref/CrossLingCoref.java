package cz.zcu.kiv.nlp.jstein.crosslingcoref;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.util.HashMap;
import java.util.Map;

import org.apache.lucene.analysis.cz.CzechStemmer;

import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Article;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Corpus;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Mention;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.io.DataHandler;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing.AdvancedTokenizer;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing.CzechStemmerAgressive;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing.CzechStemmerLight;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing.SlovakStemmer;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.preprocessing.Stemmer;

public class CrossLingCoref 
{
	private final String DATA_DIR = "data/mentions";
	private final String OUT_DIR = "out";
	
	public void run() throws IOException
	{
		DataHandler dataHandler = new DataHandler();
		Map<String, Corpus> corpora = new HashMap<String, Corpus>();		
		corpora.putAll(dataHandler.loadAnnotations(DATA_DIR));

		Map<String, Integer> entityIDs = new HashMap<String, Integer>();
		int curID = 1;
		
		for (String corpusID : corpora.keySet())
		{
			System.out.println(corpusID);

			Corpus corpus = corpora.get(corpusID);

			for (String lang : corpus.getArticles().keySet())
			{
				System.out.println("\t" + lang);

				Map<String, Article> articles = corpus.getArticles().get(lang);
	
				for (String articleID : articles.keySet())
				{
					System.out.println("\t\t" + articleID);

					Article article = articles.get(articleID);
					
			    	String outDir = OUT_DIR + File.separator + corpusID + File.separator + lang + File.separator;
			    	String outFilePath = outDir + article.getFileName();
			    	File dirFile = new File(outDir);
					if (!dirFile.exists())
						dirFile.mkdirs();
				    BufferedWriter out = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(outFilePath),"UTF8"));
				    out.write(article.getId() + "\n");
				    
					for (Mention mention : article.getMentions())
					{
						String mentionText = mention.getText();
						
						mentionText = preprocess(mentionText, lang);
						mention.setNormalized(mentionText);
						
						if (entityIDs.containsKey(mentionText))
							mention.setId(String.valueOf(entityIDs.get(mentionText)));
						else
						{
							entityIDs.put(mentionText, curID);
							mention.setId(String.valueOf(curID));
							curID++;
						}
						
						out.write(mention.getText() + "\t" + mention.getNormalized() + "\t" + mention.getType() + "\t" + mention.getId() + "\n");
					}
					
					out.close();
				}
			}
		}
		
	}
	
	private String preprocess(String text, String lang) {
		text = text.toLowerCase();
		
		Stemmer stemmer = new CzechStemmerLight();;
		if(lang.equals("cs")) stemmer = new CzechStemmerAgressive();
		if(lang.equals("sk")) stemmer = new SlovakStemmer();
		if(lang.equals("pl")) stemmer = new SlovakStemmer();
		
		text = stemmer.stem(text);
		
		text = AdvancedTokenizer.removeAccents(text);
		
		return text;
	}
	
	public static void main(String[] args) throws IOException 
	{
		CrossLingCoref coref = new CrossLingCoref();
		coref.run();
	}
}
