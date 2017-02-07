package game;

import java.awt.*;

/**
 * Reprezentuje jednotliva policka na hracim planu
 * @author Michael Hadacek A11B0359P
 *
 */
public class Policko {
	/** Souradnice policka*/
	private final int x,y;
	
	/** Index policka v hracim planu */
	private final int index;
	
	/** Figurka, ktera se na tomto policku nachazi */
	private Figurka figurka;
	
	/** Typ policka DOM, CIL, POL*/
	private typPolicka typ;
	
	/** Velikost policka v grafice */
	private int velikost = 50;
	
	/**
	 * Vytvori policko typu POL na zadanych souradnicich a zadaneho indexu
	 * @param x x-ova souradnice policka
	 * @param y y-ova souradnice policka
	 * @param index index policka
	 */
	public Policko(int x, int y, int index) 
	{
		this.x = x;
		this.y = y;
		this.index = index;
		typ = typPolicka.POL;
		
	}

	/**
	 * Vytvori policko libovolneho typu na urcenych souradnicich a indexu
	 * @param x x-ova souradnice policka
	 * @param y y-ova souradnice policka
	 * @param index index policka
	 * @param typ typ policka POL, CIL, DOM
	 */
	public Policko(int x, int y, int index, typPolicka typ) 
	{
		this(x,y,index);
		this.typ = typ;
		
	}
	
	/**
	 * Vykresli policko
	 * @param g instance Graphics
	 */
	public void draw(Graphics2D g)
	{
		g.drawOval(getX(), getY(), velikost, velikost);
	}
	
	/**
	 * Vybarvi policko
	 * @param g instance Graphics
	 */
	public void vybarvi(Graphics2D g)
	{
		g.fillOval(getX(), getY(), velikost, velikost);
		g.setColor(Color.BLACK);
		draw(g);
	}
	
	
	//Gettry a settry
	public typPolicka getTyp() {
		return typ;
	}

	public void setTyp(typPolicka typ) {
		this.typ = typ;
	}

	public int getIndex() {
		return index;
	}

	public Figurka getFigurka() {
		return figurka;
	}

	public void setFigurka(Figurka figurka) {
		this.figurka = figurka;
	}


	public int getX() {
		return x;
	}


	public int getY() {
		return y;
	}


	public boolean isObsazeno() {
		if(this.figurka == null)
			return false;
		return true;
	}

	
	

}

enum typPolicka {POL, DOM, CIL}
