package game;

import java.awt.Color;

/**
 * Reprezentuje jednotlive hrace
 * @author Michael Hadacek A11B0359P
 *
 */
public class Hrac {

	/** Barva hracovych figurek*/
	private Color barva;
	
	/** ID hrace */
	private String ID;
	
	/** Jmeno hrace*/
	private String jmeno;
	
	/** Domecek hrace */
	private Policko[] domecek;
	
	/** Policka cile hrace */
	private Policko[] cil;
	
	/** Figurky hrace */
	private Figurka[] figurky;
	
	/** Instance desky s grafikou*/
	private Deska deska;

	/** Index, na ktery hrac nasazuje sve figurky*/
	private int pocIndex;
	
	/**
	 * Vytvori hrace a urci jeho atributy
	 * @param jmeno jmeno hrace
	 * @param Barva barva figurek hrace
	 * @param pocIndex pocatecni index
	 * @param ID ID hrace
	 */
	public Hrac(String jmeno, Color Barva, int pocIndex, String ID) {
		this.barva = Barva;
		this.pocIndex = pocIndex;
		this.ID = ID;
		this.jmeno = jmeno;
	}
	
	/**
	 * Vytvori figurky hraci
	 */
	public void vytvorFigurky()
	{
		figurky = new Figurka[domecek.length];
		for(int i = 0; i < domecek.length; i++)
		{
			figurky[i] = new Figurka(this, domecek[i], deska);
		}
	}
	
	/**
	 * Vraci prvni volnou pozici v domecku
	 * @return prvni neobsazena pozice v domecku
	 */
	public Policko getVolnyDomecek()
	{
		for(Policko policko: domecek)
			if(!policko.isObsazeno())
				return policko;
		
		return null;
	}

	//Gettry a settry
	public String getJmeno() {
		return jmeno;
	}

	public void setJmeno(String jmeno) {
		this.jmeno = jmeno;
	}

	
	public int getPocIndex() {
		return pocIndex;
	}

	public Policko[] getDomecek() {
		return domecek;
	}

	public void setDomecek(Policko[] domecek) {
		this.domecek = domecek;
	}

	public Policko[] getCil() {
		return cil;
	}

	public void setCil(Policko[] cil) {
		this.cil = cil;
	}

	public Color getBarva() {
		return barva;
	}

	public void setBarva(Color barva) {
		this.barva = barva;
	}

	public Figurka[] getFigurky() {
		return figurky;
	}

	public void setFigurky(Figurka[] figurky) {
		this.figurky = figurky;
	}
	
	public String getID() {
		return ID;
	}

	public void setID(String iD) {
		ID = iD;
	}
	
	public Deska getDeska() {
		return deska;
	}

	public void setDeska(Deska deska) {
		this.deska = deska;
	}

	
	@Override
	public boolean equals(Object obj) {
		Hrac other = (Hrac)obj;
		if(other.getID().compareTo(getID()) == 0)
			return true;
		return false;
	}
	
	
	
	
}
