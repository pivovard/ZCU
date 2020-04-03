package cz.zcu.kiv.nlp.jstein.crosslingcoref.domain;

import java.util.ArrayList;
import java.util.List;

public class Article 
{
	private String id;
	private String fileName;
	private List<Mention> mentions;
	
	public String getId() 
	{
		return id;
	}
	
	public void setId(String id) 
	{
		this.id = id;
	}
	
	public List<Mention> getMentions() 
	{
		return mentions;
	}
	
	public void setMentions(List<Mention> mentions) 
	{
		this.mentions = mentions;
	}
	
	public String getFileName() 
	{
		return fileName;
	}

	public void setFileName(String fileName) 
	{
		this.fileName = fileName;
	}

	public Article()
	{
		mentions = new ArrayList<Mention>();
		this.id = "";
	}
}
