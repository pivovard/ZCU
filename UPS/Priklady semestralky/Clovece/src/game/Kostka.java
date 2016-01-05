package game;

import java.awt.*;
import java.awt.geom.*;
import java.util.Observable;
import java.util.Random;

public class Kostka extends Observable{
	
	/** Instance desky s grafikou*/
	private Deska deska;
	
	/** Urcuje zda-li je kostka hozna, tj. ma nastavenou hodnotu*/
	private boolean vrzena = false;
	
	/** Staticky urcene souradnice, kde se kostka vykresli*/
	private int x = 20;
	private int y = 20;
	
	/** Tloustka cary obrysu kostky */
	private int lineWidth = 2;
	
	/** Sirka kostky */
	private int width = 70;
	
	/** Sirka puntiku na kostce */
	private int dotWidth = 10;
	
	/** Kolik je na kostce hozeno*/
	private int hozeno;
	
	/**
	 * Vytvori novou instanci kostky 
	 * @param deska deska, na ktere kostka urcuje tahy
	 */
	public Kostka(Deska deska) {
		this.deska = deska;
	}
	
	/**
	 * Vykresli kostku
	 * @param g instance Graphics
	 */
	public void draw(Graphics g)
	{
		g.setColor(Color.white);
		g.fillRect(x+lineWidth, y+lineWidth, width - lineWidth, width - lineWidth);
		((Graphics2D) g).setStroke(new BasicStroke(2));
		g.setColor(Color.black);
		g.drawRect(x, y, width, width);
		
		switch(hozeno)
		{
			case 1: jednicka(g);
				break;
			case 2: dvojka(g);
				break;
			case 3: trojka(g);
				break;
			case 4: ctyrka(g);
				break;
			case 5: petka(g);
				break;
			case 6: sestka(g);
				break;
		}


	}
	
	/**
	 * Animuje hozeni kostkou
	 * @param g instance Graphics
	 */
	public void animuj(Graphics g)
	{
		Random r = new Random();
		
		int kolikrat = r.nextInt(10)+10;
		for(;kolikrat > 0; kolikrat --)
		{
			g.setColor(Color.white);
			g.fillRect(x+lineWidth, y+lineWidth, width - lineWidth, width - lineWidth);
			((Graphics2D) g).setStroke(new BasicStroke(2));
			
			g.setColor(Color.black);
			g.drawRect(x, y, width, width);	
			
			switch(r.nextInt(6)+1)
			{
			case 1: jednicka(g);
			break;
			case 2: dvojka(g);
			break;
			case 3: trojka(g);
			break;
			case 4: ctyrka(g);
			break;
			case 5: petka(g);
			break;
			case 6: sestka(g);
			break;
			}
			
			try {
				Thread.sleep(100);
			} catch (InterruptedException e) {
				System.out.println("Chyba sleep");
		}}
	}
	
	/**
	 * Odesle zpravu se zadosti o hozeni kostky
	 */
	public void hod()
	{
		clientTCP client = deska.getPanel().getClient();
		client.odesli("kostka");
	}
	
	/**
	 * Zobrazi na kostce jednicku
	 * @param g Instance Graphics
	 */
	public void jednicka(Graphics g)
	{
		g.setColor(Color.black);
		g.fillOval(x + width/2 - dotWidth/2, y + width/2 - dotWidth/2, dotWidth, dotWidth);
	}
	
	/**
	 * Zobrazi na kostce dvojku
	 * @param g Instance Graphics
	 */
	public void dvojka(Graphics g)
	{
		g.setColor(Color.black);
		g.fillOval(x + width/4 - dotWidth/2, y + width/4 - dotWidth/2, dotWidth, dotWidth);
		g.fillOval(x + width*3/4 - dotWidth/2, y + width*3/4 - dotWidth/2, dotWidth, dotWidth);
	}
	
	/**
	 * Zobrazi na kostce trojku
	 * @param g Instance Graphics
	 */
	public void trojka(Graphics g)
	{
		jednicka(g);
		dvojka(g);
	}
	
	
	/**
	 * Zobrazi na kostce ctyrku
	 * @param g Instance Graphics
	 */
	public void ctyrka(Graphics g)
	{
		dvojka(g);
		g.fillOval(x + width*3/4 - dotWidth/2, y + width/4 - dotWidth/2, dotWidth, dotWidth);
		g.fillOval(x + width/4 - dotWidth/2,  y + width*3/4 - dotWidth/2, dotWidth, dotWidth);
	}
	
	/**
	 * Zobrazi na kostce petku
	 * @param g Instance Graphics
	 */
	public void petka(Graphics g)
	{
		ctyrka(g);
		jednicka(g);
	}
		
	/**
	 * Zobrazi na kostce sestku
	 * @param g Instance Graphics
	 */
	public void sestka(Graphics g)
	{
		ctyrka(g);
		g.fillOval(x + width*3/4 - dotWidth/2, y + width/2 - dotWidth/2, dotWidth, dotWidth);
		g.fillOval(x + width/4 - dotWidth/2, y + width/2 - dotWidth/2, dotWidth, dotWidth);
	}
	
	/**
	 * Vraci zda je zadany bod uvnitr kostky
	 * @param p zadany bod
	 * @return true nebo false
	 */
	public boolean contains(Point p)
	{
		Rectangle2D kostka = new Rectangle2D.Float(x, x, width, width);
		return kostka.contains(p);
	}
	
	
	//Gettry a settry
	public boolean isVrzena() {
		return vrzena;
	}

	public void setVrzena(boolean vrzena) {
		this.vrzena = vrzena;
		setChanged();
		notifyObservers();
		//System.out.println("Funguje");
	}

	public int getHozeno() {
		return hozeno;
	}

	public void setHozeno(int hozeno) {
		if(hozeno > 0)
		{
			animuj(deska.getGraphics());
			deska.repaint();
		}
		this.hozeno = hozeno;
	}

}
