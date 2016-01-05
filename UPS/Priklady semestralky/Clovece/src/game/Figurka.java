package game;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Polygon;
import java.awt.geom.Ellipse2D;

/***
 * Predstavuje jednu figurku hry "Clovece, nezlob se!"
 * @author Michael Hadacek A11B0359P
 *
 */
public class Figurka {
	
	/** Pocet policek na desce */
	private static final int pocetTahu = 32;
	
	/** Hrac, kteremu patri tato figurky*/
	private Hrac hrac;
	
	/** Policko, na kterem figurka stoji*/
	private Policko policko;
	
	/** Kolikrat se jiz figurka posunula*/
	private int pocetPosunu = 0;
	
	/** Instance desky s grafikou*/
	private Deska deska;
	
	/** Elipsa realizujici hlavicku figurky v grafice*/
	private Ellipse2D hlava;
	
	/** Trojuhelnik realizujici telicko figurky v graice */
	private Polygon telo;
	
	/****************************
	 * Vytvori novou figurku a priradi hrace, policko a nastavi desku
	 * @param hrac hrac, kteremu figurka patri
	 * @param policko policko, na kterem figurka stoji
	 * @param deska deska, na ktere je figurka vykreslena
	 */
	public Figurka(Hrac hrac, Policko policko, Deska deska) 
	{
		this.hrac = hrac;
		this.policko = policko;
		policko.setFigurka(this);
		this.deska = deska;
	}

	/**
	 * Vykresli figurku
	 */
	public void draw(Graphics2D g)
	{
		int x = getPolicko().getX()+10;
		int y = getPolicko().getY()+35;
		int sirka = 30;
		int vyska = 45;
		int hlavaSirka = 25;
		int []xes = {x,x+sirka,x+sirka/2};
		int []ys = {y,y, y-vyska+5};
		
		hlava = new Ellipse2D.Double(x+2, y-vyska, hlavaSirka, hlavaSirka);
		telo = new Polygon(xes, ys, 3);
		
		g.setColor(Color.BLACK);
		g.setStroke(new BasicStroke(4));
		g.draw(hlava);
		g.setStroke(new BasicStroke(2));
		
		g.setColor(hrac.getBarva());
		g.fill(telo);
		
		g.setColor(Color.BLACK);
		g.draw(telo);
		
		g.setColor(hrac.getBarva());
		g.setStroke(new BasicStroke(1));
		g.fill(hlava);
	}
	
	/**
	 * Posune figurku o jedno policko
	 */
	public void posun()
	{
		int nextIndex = (policko.getIndex() + 1) % pocetTahu;
		
		switch(policko.getTyp())
		{
			case CIL:  {
				if(nextIndex < hrac.getCil().length)
				{
					this.policko = hrac.getCil()[nextIndex];
				}
				break;		}
			case POL: {
				if(pocetPosunu >= pocetTahu)
				{
					this.policko = hrac.getCil()[0];
				}
				else
				{
					this.policko = deska.getPlan()[nextIndex];
				}
				break;
					}
			case DOM:
			{
				this.policko = deska.getPlan()[hrac.getPocIndex()];
			}
		}
		
		pocetPosunu++;

		
		
	}
	
	/**
	 * Posune figurku o urceny pocet posunu
	 * @param kolikrat kolikrat se ma figurka posunout
	 */
	public void posun(int kolikrat)
	{
		if(kontrolaPosunu(kolikrat))
		{
			if(policko.getTyp() == typPolicka.DOM)
				kolikrat = 1;
			policko.setFigurka(null);
			for(int i = 0; i < kolikrat; i++)
			{
				posun();
			}

			vyhod(policko);
		}
	}
	
	/**
	 * Vyhodi figurky podle pravidel hry
	 * @param policko policko na ktere se figurka presouva
	 */
	public void vyhod(Policko policko)
	{
		if(policko.isObsazeno())
		{
			Figurka stara = policko.getFigurka();
			Policko domecek = stara.getHrac().getVolnyDomecek();
			stara.setPolicko(domecek);
			domecek.setFigurka(stara);
			stara.setPocetPosunu(0);
		}
		policko.setFigurka(this);
	}
	
	
	/**
	 * Zkontroluje, zda je posun figurky mozny
	 * @param kolik o kolik se ma figurka posunout
	 * @return true, je-li posun figurky mozny, false jinak
	 */
	public boolean kontrolaPosunu(int kolik)
	{
		if(policko.getTyp() == typPolicka.DOM && kolik != 6)
			return false;
		
		if(pocetPosunu + kolik <= pocetTahu)
			return true;
		else if(pocetPosunu + kolik <= pocetTahu + hrac.getCil().length && !hrac.getCil()[(pocetPosunu + kolik) % pocetTahu - 1].isObsazeno())
			return true;
		return false;
		
		
	}
	
	/**
	 * Vraci, zda se dany bod nachazi uvnitr nebo vne figurky
	 * @param p bod na ktery se ptame
	 * @return true, je-li bod uvnitr figurky, false jinak
	 */
	public boolean contains(Point p)
	{
		return hlava.contains(p) || telo.contains(p);
	}
	
	//Gettry a settry
	public Policko getPolicko() {
		return policko;
	}


	public void setPolicko(Policko policko) {
		this.policko = policko;
	}
	
	public Hrac getHrac() {
		return hrac;
	}


	public int getPocetPosunu() {
		return pocetPosunu;
	}


	public void setPocetPosunu(int pocetPosunu) {
		this.pocetPosunu = pocetPosunu;
	}

}
