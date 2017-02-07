package game;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.Observable;
import java.util.Observer;
import javax.imageio.ImageIO;
import javax.swing.*;

/**
 * Realizuje grafiku pro zobrazovani hry "Clovece, nezlob se!"
 * @author Michael Hadacek A11B0359P
 *
 */
public class Deska extends JPanel implements Observer
{
	/** ID pro graphics	 */
	private static final long serialVersionUID = 1L;

	/** Instance hlavniho okna */
	private HlavniPanel panel;

	/** Urcuje zda byla ci nebyla kostka hozena*/
	private boolean kostkaVrzena = false;
	
	//Domecek,cil a index zeleneho hrace
	private Policko [] zelenyDom;
	private Policko [] zelenyCil;
	private int zelenyIndex = 0;
	private boolean zeleny = false;
	
	//Domecek,cil a index cerveneho hrace
	private Policko [] cervenyDom;
	private Policko [] cervenyCil;
	private int cervenyIndex = 8;
	private boolean cerveny = false;
	
	//Domecek,cil a index zluteho hrace
	private Policko [] zlutyDom;
	private Policko [] zlutyCil;
	private int zlutyIndex = 24;
	private boolean zluty = false;
	
	//Domecek,cil a index modreho hrace
	private Policko [] modryDom;
	private Policko [] modryCil;
	private int modryIndex = 16;
	private boolean modry = false;
	
	/** Instance obrazku na pozadi desky*/
	private BufferedImage img = null;
	
	/** Pole vsech policek na hraci desce*/
	private Policko [] plan;
	
	/** Seznam hrajicich hracu*/
	private ArrayList<Hrac> hraci = new ArrayList<Hrac>();
	
	/** Instance kostky */
	private Kostka kostka;
	
	/** Hrac vlastnici danou instanci desky*/
	private Hrac me;
	
	/**
	 * Nacte obrazek na pozadi desky
	 * @return obrazek pozadi
	 */
	public BufferedImage nactiObrazek()
	{
		BufferedImage img = null;
		try 
		{
			img = ImageIO.read(getClass().getResource("/board.jpg"));
		} catch (Exception e) 
		{
			System.out.println("Chybka se vloudila");
			img = null;
		}
		
		return img;
	}
	
	/************************
	 * Vytvori instanci desky a priradi vsechny potrebne atributy
	 * @param panel hlavni panel ve kterem se deska bude zobrazovat 
	 */
	public Deska(HlavniPanel panel) 
	{
		this.panel = panel;
		kostka = new Kostka(this);
		kostka.addObserver(this);
		img = nactiObrazek();
		
		vytvorPlan();
		vytvorPolicka();	
		setVisible(false);

		this.addMouseListener(new MouseAdapter() {
			public void mouseClicked(MouseEvent e) 
			{

				if(kostkaVrzena)
				{
					Figurka figurky[] = getMe().getFigurky();
					for(int i = 0; i < figurky.length; i++)
					{
						if(figurky[i].contains(e.getPoint()))
						{
							clientTCP client = getPanel().getClient(); 
							client.odesli("figurka;"+i);

							repaint();
						}
					}


				}
				else if(kostka.contains(e.getPoint()) /*&& odehrano*/)
				{
					kostka.hod();
					repaint();
				}
			}
		});
	}


	/**
	 * Vytvori policka pro domecky a cile vsech barev
	 */
	public void vytvorPolicka()
	{
		int posun = 60;
		int posunMaly = 30;

		zelenyDom = new Policko[4];
		zelenyCil = new Policko[4];

		Policko ref = plan[plan.length-1];

		for(int i = 0; i < zelenyCil.length; i++)
		{
			zelenyCil[i] = new Policko(ref.getX()+(i+1)*(posun-3), ref.getY(), i);
			zelenyCil[i].setTyp(typPolicka.CIL);
		}
		zelenyDom[0] = new Policko(ref.getX()+posunMaly, ref.getY() - 3*posun,0);
		zelenyDom[1] = new Policko(ref.getX()+posunMaly + posun, ref.getY() - 3*posun,1);
		zelenyDom[2] = new Policko(ref.getX()+posunMaly, ref.getY() - 4*posun,2);
		zelenyDom[3] = new Policko(ref.getX()+posunMaly + posun, ref.getY() - 4*posun,3);

		cervenyDom = new Policko[4];
		cervenyCil = new Policko[4];

		ref = plan[cervenyIndex-1];

		for(int i = 0; i < cervenyCil.length; i++)
		{
			cervenyCil[i] = new Policko(ref.getX(), ref.getY()+(i+1)*(posun-3), i);
			cervenyCil[i].setTyp(typPolicka.CIL);
		}

		cervenyDom[0] = new Policko(ref.getX() + 3*posun, ref.getY()+posunMaly,0);
		cervenyDom[1] = new Policko(ref.getX() + 3*posun, ref.getY()+posunMaly + posun,1);
		cervenyDom[2] = new Policko(ref.getX() + 4*posun, ref.getY()+posunMaly,2);
		cervenyDom[3] = new Policko(ref.getX() + 4*posun, ref.getY()+posunMaly + posun,3);

		zlutyDom = new Policko[4];
		zlutyCil = new Policko[4];
		ref = plan[zlutyIndex-1];
		for(int i = 0; i < zlutyCil.length; i++)
		{

			zlutyCil[i] = new Policko(ref.getX(), ref.getY()-(i+1)*(posun-3), i);
			zlutyCil[i].setTyp(typPolicka.CIL);
		}

		zlutyDom[0] = new Policko(ref.getX() - 3*posun, ref.getY()-posunMaly,0);
		zlutyDom[1] = new Policko(ref.getX() - 3*posun, ref.getY()-posunMaly - posun,1);
		zlutyDom[2] = new Policko(ref.getX() - 4*posun, ref.getY()-posunMaly,2);
		zlutyDom[3] = new Policko(ref.getX() - 4*posun, ref.getY()-posunMaly - posun,3);

		modryDom = new Policko[4];
		modryCil = new Policko[4];
		ref = plan[modryIndex-1];
		for(int i = 0; i < modryCil.length; i++)
		{

			modryCil[i] = new Policko(ref.getX()-(i+1)*(posun-3), ref.getY(), i);
			modryCil[i].setTyp(typPolicka.CIL);
		}
		modryDom[0] = new Policko(ref.getX()-posunMaly, ref.getY() + 3*posun,0);
		modryDom[1] = new Policko(ref.getX()-posunMaly - posun, ref.getY() + 3*posun,1);
		modryDom[2] = new Policko(ref.getX()-posunMaly, ref.getY() + 4*posun,2);
		modryDom[3] = new Policko(ref.getX()-posunMaly - posun, ref.getY() + 4*posun,3);	

		for(int i = 0; i < modryCil.length; i++)
		{
			modryDom[i].setTyp(typPolicka.DOM);
			zelenyDom[i].setTyp(typPolicka.DOM);
			zlutyDom[i].setTyp(typPolicka.DOM);
			cervenyDom[i].setTyp(typPolicka.DOM);
		}

	}
	
	/**
	 * Odebere hrace
	 * @param hrac hrac, ktery se ma odebrat
	 */
	public void odeberHrace(Hrac hrac)
	{
		if(hrac != null)
		{
			hraci.remove(hrac);

			if(hrac.getBarva() == Color.GREEN)
				zeleny = false;
			if(hrac.getBarva() == Color.RED)
				cerveny = false;
			if(hrac.getBarva() == Color.BLUE)
				modry = false;
			if(hrac.getBarva() == Color.YELLOW)
				zluty = false;

			repaint();
		}
	}
	
	
	/**
	 * Prida hrace a priradi mu pocatecni index, domecek a cil
	 * @param jmeno jmeno hrace
	 * @param barva barva hrace
	 * @param ID ID hrace
	 * @return vytvoreny hrac
	 */
	public Hrac pridejHrace(String jmeno, String barva, String ID)
	{
		Color color = null;
		Policko [] domecek = null;
		Policko [] cil = null;
		int index = 0;
		
		if(barva.compareTo("zlu") == 0 && !zluty)
		{
			color = Color.YELLOW;
			domecek = zlutyDom;
			cil = zlutyCil;
			index = zlutyIndex;
			zluty = true;
		}
		if(barva.compareTo("cer") == 0 && !cerveny)
		{
			color = Color.RED;
			domecek = cervenyDom;
			cil = cervenyCil;
			index = cervenyIndex;
			cerveny = true;
		}
		if(barva.compareTo("mod") == 0 && !modry)
		{
			color = new Color(0,250,250);
			domecek = modryDom;
			cil = modryCil;
			index = modryIndex;
			modry = true;
		}
		if(barva.compareTo("zel") == 0 && !zeleny)
		{
			color = Color.GREEN;
			domecek = zelenyDom;
			cil = zelenyCil;
			index = zelenyIndex;
			zeleny = true;
		}
		
		if(color == null)
			return null;
		
		Hrac novy = new Hrac(jmeno, color, index, ID);
		novy.setCil(cil);
		novy.setDomecek(domecek);
		novy.setDeska(this);
		novy.vytvorFigurky();
		
		if(hraci.contains(novy))
		{
			odeberHrace(hraci.get(hraci.indexOf(novy)));
		}
		
		hraci.add(novy);
		repaint();
		
		return novy;
	}
	
	
	/**
	 * Vykresli desku
	 */
	@Override
	public void paint(Graphics g) {
		Graphics2D g2 = (Graphics2D)g;
		g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
		paintBackground(g);
		vykresliPolicka(g2);
			
		if(hraci.size() > 0)
			for(Hrac hrac: hraci)
			{

				for(Figurka fig: hrac.getFigurky())
				{
					fig.draw(g2);
				}
			}

		kostka.draw(g);

	}
	
	/**
	 * Vykresli policka cilu a domecku
	 * @param g2 instance {@code Graphics2D}
	 */
	public void vykresliPolicka(Graphics2D g2)
	{
		
		g2.setStroke(new BasicStroke(2));
		
		g2.setColor(Color.BLACK);
		for(Policko policko: plan)
			policko.draw(g2);
		
		for(Policko pol: zelenyDom)
		{
			g2.setColor(new Color(0, 255, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: cervenyDom)
		{
			g2.setColor(new Color(255, 0, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: zlutyDom)
		{
			g2.setColor(new Color(255, 255, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: modryDom)
		{
			g2.setColor(new Color(0,250,250,150));
			pol.vybarvi(g2);
		}
		for(Policko pol: zelenyCil)
		{
			g2.setColor(new Color(0, 255, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: cervenyCil)
		{
			g2.setColor(new Color(255, 0, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: zlutyCil)
		{
			g2.setColor(new Color(255, 255, 0, 150));
			pol.vybarvi(g2);
		}
		for(Policko pol: modryCil)
		{
			g2.setColor(new Color(0,250,250,150));
			pol.vybarvi(g2);
		}
	}
	
	/**
	 * Vykresli obrazek pozadi
	 * @param g instance {@code Graphics}
	 */
	public void paintBackground(Graphics g)
	{
		if(img != null)
			g.drawImage(img, 0, 0,800,800, null);
		else g.clearRect(0, 0, getWidth(), getHeight());
	}
	
	/**
	 * Vytvori herni plan
	 * Souradnice a parametry jsou staticky zadany
	 */
	public void vytvorPlan()
	{
		plan = new Policko[32];
		int x = 120; 
		int y = 250;
		int posun = 60;
		int posunMaly = 30;
		int i;

		for(i = 0; i < 4; i++)
		{
			plan[i] = new Policko(x,y,i);
			x += posun;
		}

		x -= posun;
		y -= posun;
		for(; i < 7; i++)
		{
			plan[i] = new Policko(x,y,i);
			y -= posun;

		}

		y += posunMaly;
		x += posun;
		plan[i] = new Policko(x,y,i++);

		y += posunMaly;
		x += posun;

		for(; i < 11; i++)
		{
			plan[i] = new Policko(x,y,i);
			y += posun;

		}

		for(; i < 15; i++)
		{
			plan[i] = new Policko(x,y,i);
			x += posun;

		}

		y += posun;
		x -= posunMaly;
		plan[i] = new Policko(x,y,i++);

		y += posun;
		x -= posunMaly;
		for(; i < 19; i++)
		{
			plan[i] = new Policko(x,y,i);
			x -= posun;

		}

		for(; i < 23; i++)
		{
			plan[i] = new Policko(x,y,i);
			y += posun;

		}

		y -= posunMaly;
		x -= posun;
		plan[i] = new Policko(x,y,i++);

		y -= posunMaly;
		x -= posun;

		for(; i < 27; i++)
		{
			plan[i] = new Policko(x,y,i);
			y -= posun;

		}

		for(; i < 31; i++)
		{
			plan[i] = new Policko(x,y,i);
			x -= posun;

		}

		y -= posun;
		x += posunMaly;
		plan[i] = new Policko(x,y,i);
		
	}
	
	/**
	 * Prekresli desku a nastavi kostku na novou hodnotu
	 */
	@Override
	public void update(Observable o, Object arg) {
		if(kostka.isVrzena())
		{	
			repaint();
		}
		kostkaVrzena = kostka.isVrzena();
		
	}
	
	//Gettry a settry
	public Hrac getHrac(String id)
	{
		for(Hrac hrac: hraci)
		{
			//System.out.println(hrac.getID()+ " x "+id);
			if(hrac.getID().compareTo(id) == 0)
				return hrac;
		}
		return null;
	}
	
	public Hrac getMe() {
		return me;
	}

	public void setMe(Hrac me) {
		this.me = me;
	}

	public Kostka getKostka() {
		return kostka;
	}

	public void setKostka(Kostka kostka) {
		this.kostka = kostka;
	}

	public HlavniPanel getPanel() {
		return panel;
	}

	public void setPanel(HlavniPanel panel) {
		this.panel = panel;
	}
	
	public Policko[] getPlan() {
		return plan;
	}

	public void setPlan(Policko[] plan) {
		this.plan = plan;
	}
}
