package cz.zcu.kiv.nlp.jstein.crosslingcoref.domain;

public class Mention 
{
	private String id;
	private String text;
	private String normalized;
	private EntityType type;
	
	public String getId() 
	{
		return id;
	}
	
	public void setId(String id) 
	{
		this.id = id;
	}
	
	public String getText() 
	{
		return text;
	}
	
	public void setText(String text) 
	{
		this.text = text;
	}
	
	public String getNormalized() 
	{
		return normalized;
	}
	
	public void setNormalized(String normalized) 
	{
		this.normalized = normalized;
	}
	
	public EntityType getType() 
	{
		return type;
	}
	
	public void setType(EntityType type) 
	{
		this.type = type;
	}
	
	public String toString()
	{
		return text + "\t" + normalized + "\t" + type + "\t" + id;
	}
}
