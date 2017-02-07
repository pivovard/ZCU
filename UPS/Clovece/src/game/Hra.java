package game;

/**
 * Prepravka
 * Uchovava informace o hre
 * Uchovava ID a jmeno hry
 * @author Michael Hadacek A11B0359P
 *
 */
public class Hra {
	/** ID hry */
	public String id;
	
	/** Jmeno hry*/
	public String jmeno;
	
	/**
	 * Vytvori instanci se zadanym ID a jmenem
	 * @param id id
	 * @param jmeno jmeno
	 */
	public Hra(String id, String jmeno)
	{
		this.id = id;
		this.jmeno = jmeno;
	}
	
	/**
	 * Vrati textovou reprezentaci instance objektu
	 */
	@Override
	public String toString() {
		return jmeno;
	}

}
