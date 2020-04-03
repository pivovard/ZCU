package cz.zcu.kiv.nlp.jstein.crosslingcoref.io;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.util.HashMap;
import java.util.Map;

import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Article;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Corpus;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.EntityType;
import cz.zcu.kiv.nlp.jstein.crosslingcoref.domain.Mention;


public class DataHandler
{
	public Map<String, Corpus> loadAnnotations(String dataDir)
	{
		Map<String, Corpus> annotations = new HashMap<String, Corpus>();
			
		String systemDir = dataDir;
	    File[] corpusDirs = new File(systemDir).listFiles();
	    for (File corpusDirFile : corpusDirs) 	    	
	    {
	    	if (!corpusDirFile.isDirectory()) continue;
	    	
	    	String corpusID = corpusDirFile.getName();
	    	annotations.put(corpusID, new Corpus());
	    	annotations.get(corpusID).setId(corpusID);

			String corpusDir = systemDir + File.separator + corpusID;
		    File[] langDirs = new File(corpusDir).listFiles();
		    for (File langDirFile : langDirs) 	    	
		    {
		    	if (!langDirFile.isDirectory()) continue;
		    	
		    	String lang = langDirFile.getName();
		    	annotations.get(corpusID).getArticles().put(lang, new HashMap<String, Article>());
		    	
		    	String langDir = corpusDir + File.separator + lang;
			    File[] articleFiles = new File(langDir).listFiles();
			    for (File articleFile : articleFiles) 	    	
			    {
			    	if (!articleFile.isFile()) continue;
	
			    	String fileName = articleFile.getName();
			    	Article article = loadFile(langDir + File.separator + fileName);
			    	article.setFileName(fileName);
			    	annotations.get(corpusID).getArticles().get(lang).put(article.getId(), article);
			    }	    	
		    }
	    }		
		return annotations;
	}
	
	public Article loadFile(String filePath)
	{
		Article article = new Article();
		
		try
		{
			InputStreamReader fr = new InputStreamReader(new FileInputStream(filePath),"UTF8");
			BufferedReader in = new BufferedReader(fr);
			
			String line = "";
			while ((line = in.readLine()) != null) 
			{
				line = line.trim();			
				if (line.length() == 0) continue;
				
				if (line.indexOf("#") != -1)
					line = line.substring(0, line.indexOf("#"));
				
				if (article.getId().equals(""))
				{
					while ((line.length() > 0) && (!Character.isDigit(line.charAt(0)))) line = line.substring(1);
					String articleID = "";
					if (line.length() > 0) articleID = line;
					article.setId(articleID);
					continue;
				}
				
				Mention mention = extractLine(line);
				
				if (mention == null)
				{
					System.out.println("WARNING: line incomplete: " + line + " - " + filePath);
					continue;					
				}

				article.getMentions().add(mention);
			}		
			in.close();
		}
		catch (Exception e)
		{
			System.out.println("ERROR: loading file " + filePath + " failed");
			e.printStackTrace();
		}
		return article;
	}
	
	public Mention extractLine(String line)
	{
		String[] lineItems = line.split("\t");
		if (lineItems.length < 3) 
			return null;
		
		Mention mention = new Mention();
		mention.setText(lineItems[0].toLowerCase().trim());
		mention.setNormalized(lineItems[1].toLowerCase().trim());
		mention.setType(EntityType.valueOf(lineItems[2].trim().toUpperCase()));
		
		if (lineItems.length <= 3) 
			mention.setId("-1");
		else
			mention.setId(lineItems[3].trim());
		
		return mention;		
	}
}
