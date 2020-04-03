package cz.zcu.kiv.nlp.jstein.crosslingcoref.domain;

import java.util.HashMap;
import java.util.Map;

public class Corpus 
{
	private String id;
	private Map<String, Map<String, Article>> articles;
	
	public String getId() 
	{
		return id;
	}
	
	public void setId(String id) 
	{
		this.id = id;
	}
	
	public Map<String, Map<String, Article>> getArticles() 
	{
		return articles;
	}
	
	public void setArticles(Map<String, Map<String, Article>> articles) 
	{
		this.articles = articles;
	}
	
	public Corpus()
	{
		this.articles = new HashMap<String, Map<String, Article>>();
	}
}
