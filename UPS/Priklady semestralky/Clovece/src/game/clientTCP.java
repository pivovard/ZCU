package game;
import java.io.*;
import java.net.*;
import javax.swing.JLabel;
import javax.swing.JOptionPane;

/**
 * Klient zajistujici pripojeni a obsluze spojeni se serverem 
 * @author Michael Hadacek A11B0359P
 *
 */
class clientTCP implements Runnable
{
	
	/** Instance hlavniho okna*/
	public static HlavniPanel hra;
	
	/** Stream odchoziho spojeni */
	private OutputStream out;
	
	/** Stream prichoziho spojeni */
	private BufferedReader in;
	
	/** Socket spojeni se serverem */
	private Socket socket;
	
	/** ID klienta */ 
	private String ID;
	
	/** ID hry ve ktere se klient nachazi */
	private String hraID;
	
	/** Jmeno klienta */
	private String jmeno;

	/** Instance {@code Desky} obsahujici grafiku hry*/
	private Deska deska;
	
	/** Pocet proslych timeoutu */
	int timeout = 0;
	
	
	//Getry a setry
	public String getID() {
		return ID;
	}


	public void setID(String iD) {
		ID = iD;
	}


	public String getHraID() {
		return hraID;
	}


	public void setHraID(String hraID) {
		this.hraID = hraID;
	}


	public String getJmeno() {
		return jmeno;
	}


	public void setJmeno(String jmeno) {
		this.jmeno = jmeno;
	}


	
	
	/**
	 * Metoda prijimaci vlakna
	 * Prijima zpravy ze serveru
	 */
	@Override
	public void run() {
		String zprava = new String();

		try {
			do{
				try{
					while((zprava = in.readLine()) != null)	
					{
						zpracuj(zprava);
						zprava = new String();
					}
					timeout++;

				} catch (SocketTimeoutException e) {
					odesli("hry");
				}
			}while(timeout < 2);



			JOptionPane.showMessageDialog(hra, "Doslo k preruseni spojeni se serverem", "Chyba spojeni", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}

		catch (IOException e) {
			JOptionPane.showMessageDialog(hra, "Na serveru doslo k chybe", "Chyba spojeni", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}	
		}
	
	/**
	 * Aktualizuje label s informacemi
	 * Vypisuje informace pred zahajenim hry
	 * @param ja true, jestlize se informace tykaji me
	 * @param stav kod stavu
	 */
	public void cekej(boolean ja, int stav)
	{
		if(ja)
		{
			hra.cekej(stav);
		}
		else hra.cekej(3);
	}
	
	/**
	 * Vypise hlasku z chybou
	 */
	public void vypisChyba()
	{
		System.out.println("Nastala chyba!");
		System.out.println("Prijata zprava neodpovida formatu");
	}
	
	/**
	 * Odebere hrace ze hry
	 * @param id id odebiraneho hrace
	 */
	public void odeberHrace(String id)
	{
		if(jsemJa(id))
		{
			hra.vypniHru();
			hraID = null;
			deska = new Deska(hra);
			hra.deska = deska;
		}
		else deska.odeberHrace(deska.getHrac(id));
	}
	
	/**
	 * Nastavi label s informacemi
	 * Vypisuje, kdo je na tahu
	 * @param id id hrajiciho hrace
	 */
	public void setLabel(String id)
	{
		try{
		if(jsemJa(id))
		{
			Hrac hrac = deska.getMe();
			JLabel info = hra.getInfoLB();
			info.setBackground(hrac.getBarva());
			hra.getInfoLB().setText("Jste na tahu");
		}
		else 
		{
			Hrac hrac = deska.getHrac(id);
			JLabel info = hra.getInfoLB();
			info.setBackground(hrac.getBarva());
			info.setText("Hraje "+hrac.getJmeno());
		}
		}
		catch(NullPointerException e)
		{
			JOptionPane.showMessageDialog(hra, "V programu doslo k chybe! Aplikaci je nutne restartovat", "Chyba v programu", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}
	}
	
	/**
	 * Aktualizuje seznam her podle prijate zpravy
	 * @param priznaky prijata zprava
	 */
	public void setListOfGames(String [] priznaky)
	{
		hra.hryModel.removeAllElements();
		
		for(int i = 2; i+1 < priznaky.length; i += 2)
		{
			priznaky[i] = priznaky[i].trim();
			priznaky[i+1] = priznaky[i+1].trim();
			hra.hryModel.addElement(new Hra(priznaky[i+1], priznaky[i]));	
		}
		
		timeout = 0;
	}
	
	
	/**
	 * Zkontroluje zda muze ci nemuze hrat
	 * Jestlize ano, pohne s figurkou
	 * @param priznaky
	 */
	public void zkontrolujTah(String [] priznaky)
	{
		try{
			priznaky[0] = priznaky[0].trim();
			priznaky[2] = priznaky[2].trim();
			
			int figurka_index = Integer.parseInt(priznaky[2]);
			int policko_index = -1;

			if(priznaky[3].compareTo("ok") == 0)
			{
				if(figurka_index >= 0)
				{
					Figurka figurka = deska.getHrac(priznaky[0]).getFigurky()[figurka_index];
					figurka.posun(deska.getKostka().getHozeno());
					policko_index = figurka.getPolicko().getIndex();
					
					deska.repaint();
				}
				if(jsemJa(priznaky[0]))
					odesli("odehrano;"+figurka_index+";"+policko_index);
				
				deska.getKostka().setVrzena(false);
			}
			
		}catch(NumberFormatException e)
		{
			vypisChyba();
		}
	}
	
	/**
	 * Prida hrace podle prijate zpravy
	 * @param priznaky prijata zprava s informacemi o hracich
	 */
	public void pridejHrace(String [] priznaky)
	{
		for(int i = 2; i+2 < priznaky.length; i += 3)
		{
			priznaky[i] = priznaky[i].trim();
			priznaky[i+1] = priznaky[i+1].trim();
			priznaky[i+2] = priznaky[i+2].trim();
			deska.pridejHrace(priznaky[i+1],priznaky[i+2], priznaky[i]);
		}
	}
	
	/**
	 * Nastave hrace, ktery reprezentuje hrace, kteremu patri tento klient
	 * @param jmeno jmeno
	 * @param ID ID
	 * @param hraID ID hry, ve kterou hrac hraje
	 * @param barva barva hrace
	 */
	public void nastavMe(String jmeno, String ID, String hraID, String barva)
	{
		deska = new Deska(hra);
		hra.deska = deska;
		this.hraID = hraID;

		Hrac ja = deska.pridejHrace(jmeno, barva, ID);
		
		if(ja != null)
		{
			deska.setMe(ja);
			hra.zapniHru();
			hra.barvaDialog();
			
			odesli("stav");
			odesli("hraci");
		}
		else vypisChyba();
	}
	
	
	/**
	 * Nastavi kostku na hozenou hodnotu a zobrazi v grafice
	 * @param ja true, jestlize jsem hazel ja, false kdyz nekdo jiny
	 * @param kolik kolik bylo hozeno na kostce
	 */
	public void hodKostkou(boolean ja, String kolik)
	{
		try
		{
			deska.getKostka().setHozeno(Integer.parseInt(kolik));
		}
		catch(NumberFormatException e)
		{
			System.out.println("Vyskytla se chyba");
			System.exit(1);
		}
		
		deska.repaint();

		if(ja)
			deska.getKostka().setVrzena(true);
	}
	
	
	/**
	 * Zoobrazi dialog pro vyhru
	 * @param hrac hrac, ktery vyhral
	 */
	public void setVyhraDialog(Hrac hrac)
	{
		String msg;
		
		if(jsemJa(hrac.getID()))
			msg = "Vyhral jste!";
		else 
		{
			msg = "Vyhral ";

			if(hrac != null)
				msg += hrac.getJmeno();
		}

			hra.vyhraDialog(msg);
	}
	
	/**
	 * Zobrazi dialog pri vyrazeni ze hry kvuli timeoutu
	 */
	public void kickedDialog()
	{
		JOptionPane.showMessageDialog(hra, "Byli jste vyrazeni ze hry pro necinnost", "Odpojeni", JOptionPane.WARNING_MESSAGE);
	}
	
	/**
	 * Zobrazi dialog pri ukonceni serveru
	 */
	public void endServerDialog()
	{
		JOptionPane.showMessageDialog(hra, "Server byl ukoncen, budete automaticky odpojeni", "Ukonceni serveru", JOptionPane.WARNING_MESSAGE);
		odpoj();
	}
	
	/**
	 * Zpracuje prijatou zpravu a vyvola odpovidajici akci
	 * @param zprava prijata zprava
	 */
	public void zpracuj(String zprava)
	{
		String [] priznaky = zprava.split(";");
		//System.out.println(Arrays.toString(priznaky));
		
		if(priznaky.length > 1)
		{
			switch(cisloAkce(priznaky[1]))
			{
			case 0: vypisChyba();
				
			break;
			case 1: setVyhraDialog(deska.getHrac(priznaky[0]));
			break;
			case 2: hodKostkou(jsemJa(priznaky[0]), priznaky[2]);
			break;
			case 3: nastavMe(jmeno, priznaky[0], priznaky[3], priznaky[2]);
			break;
			case 4:	pridejHrace(priznaky);
			break;
			
			case 5:	zkontrolujTah(priznaky);
			break;
			
			case 6: setListOfGames(priznaky);
			break;

			case 7:	setLabel(priznaky[0]);
			break;

			case 8:	{
					try{
						cekej(jsemJa(priznaky[0]), Integer.parseInt(priznaky[2]));
					}catch(NumberFormatException e)
					{
						vypisChyba();
					}
			}
			break;

			case 9:	odeberHrace(priznaky[0]);
			break;
			
			case 10: System.exit(0);
			break;
			
			case 11: kickedDialog();
			break;
			
			case 12: endServerDialog();
			break;
			}

		}
		else vypisChyba();

	}
	
	/**
	 * Urci cislo akce, podle prijate zpravy
	 * @param nazev prijaty nazev akce
	 * @return cislo akce
	 */
	public int cisloAkce(String nazev)
	{
		if(nazev.compareTo("vyhra") == 0)
			return 1;
		if(nazev.compareTo("kostka") == 0)
			return 2;
		if(nazev.compareTo("ty") == 0)
			return 3;
		if(nazev.compareTo("hraci") == 0)
			return 4;
		if(nazev.compareTo("figurka") == 0)
			return 5;
		if(nazev.compareTo("hry") == 0)
			return 6;
		if(nazev.compareTo("hraj") == 0)
			return 7;
		if(nazev.compareTo("stav") == 0)
			return 8;
		if(nazev.compareTo("odpoj") == 0)
			return 9;
		if(nazev.compareTo("konec") == 0)
			return 10;
		if(nazev.compareTo("kicked") == 0)
			return 11;
		if(nazev.compareTo("konec_server") == 0)
			return 12;
		return 0;
	}
	
	/**
	 * Vytvori zpravu ve formatu prijimanem serverem
	 * @param zprava odesilana zprava
	 */
	public void odesli(String zprava)
	{
		String zpr = null;
		
		if(ID != null)
			zpr = ID + ";" + zprava; 
		else zpr = zprava;
		
		if(hraID != null)
			zpr += ";"+hraID;
		
		try {
			
			out.write(zpr.getBytes());
			
		} catch (IOException e) 
		{
			System.out.println("Neodeslano  "+zpr);
		}
	}
	
	/**
	 * Zjisti zda prijate ID se rovna s id tohoto klienta
	 * @param Id prijate ID
	 * @return true nebo false
	 */
	public boolean jsemJa(String Id)
	{
		if(Id.compareTo(ID) == 0)
			return true;
		return false;
	}
	
	
	/**
	 * Zobrazi dialog pro pripojeni
	 * Ze zadaneho dialogu nacte data a vytvori socket
	 * Po pripojeni provede ukony pro uspesny pridani klienta na server
	 * @return vytvoreny socket
	 */
	public Socket pripoj()
	{
		int port;
		
		do{
			do{
				
				hra.pripojDialog();
				try{
					port = Integer.parseInt(hra.portTF.getText());
				}catch(NumberFormatException e)
				{
					port = -1;
				}
				
			}while(hra.jmenoTF.getText().compareTo("") == 0 || hra.adresaTF.getText().compareTo("") == 0 || hra.portTF.getText().compareTo("") == 0 || port == -1);


			jmeno = hra.jmenoTF.getText();
			
			try{
				socket = new Socket(hra.adresaTF.getText(), port);
				socket.setSoTimeout(100000);
				
				InetAddress adresa = socket.getInetAddress();
				System.out.print("Pripojuju se na : "+adresa.getHostAddress()+" se jmenem : "+adresa.getHostName()+"\n" );
				
				out = socket.getOutputStream();
				in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
				
			}catch(Exception e)
			{
				JOptionPane.showMessageDialog(hra, "Nelze se pripojit k serveru", "Chyba spojeni", JOptionPane.ERROR_MESSAGE);
				socket = null;
			}
			
		}while(socket == null);

		if(jmeno.length() > 20)
			jmeno = jmeno.substring(0,20);

		if(jmeno == null)
			System.exit(0);
		
		else odesli("1;jmeno;"+jmeno+";");

		try{
			
			String zprava;
			if((zprava = in.readLine()) != null)
			{
				String [] priznaky = zprava.split(";");
				
				if(priznaky.length > 1 && priznaky[1].compareTo("ID")==0)
					ID = priznaky[0].trim();
				else System.exit(1);
			}

			odesli("hry");
			
		}catch(Exception e)
		{
			JOptionPane.showMessageDialog(hra, "Pri pripojovani doslo k chybe! Restartujte aplikaci", "Chyba spojeni", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}
		return socket;
	}
	
	
	/**
	 * Zavre socket, vstupni a vystupni stream
	 * @throws Exception
	 */
	public void odpoj()
	{
		try{
			in.close();
			out.close();
			socket.close();
		}catch(IOException e)
		{
			System.out.println("Nepodarilo se zavrit socket nebo streamy");
		}
	}
	
	/**
	 * Hlavni metoda aplikace, vytvori klienta a zobrazi formular pro pripojeni
	 * @param args nepouzit
	 * @throws Exception
	 */
	public static void main(String [] args) 
	{
	
		clientTCP client = new clientTCP();
		hra = new HlavniPanel();
		client.pripoj();		
		hra.setVisible(true);

		hra.setClient(client);
		client.deska = hra.getDeska();
		
		Thread reader = new Thread(client);
		reader.start();
		
		//Thread game = new Thread(client);
		//game.start();


	}
}
