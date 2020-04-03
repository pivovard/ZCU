package cz.zcu.kiv.nlp.jstein.crosslingcoref.domain;

public enum EntityType 
{
	PER ("PER"),
	LOC ("LOC"), 
	ORG ("ORG"), 
	MISC ("MISC");
	
    private final String name;       

    private EntityType(String s) 
    {
        this.name = s;
    }

    public boolean equalsName(String otherName) 
    {
        return name.equals(otherName);
    }

    public String toString() 
    {
       return this.name;
    }
    
    public static boolean exists(String type)
    {
        for (EntityType enumEntityType : EntityType.values()) 
            if (enumEntityType.name().equalsIgnoreCase(type))
                return true;
        return false;
    }
}
