package game;

import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

import javax.swing.*;

/**
 * Hlavni okno aplikace
 * @author Michael Hadacek A11B0359P
 *
 */
public class HlavniPanel extends JFrame
{
	/** ID */
	private static final long serialVersionUID = 1L;

	/** Instance desky s grafikou */
	public Deska deska;
	
	/** Textfield pro nacteni IP adresy serveru pro pripojeni*/
	public JTextField adresaTF = new JTextField();
	
	/** Textfield pro nacteni jmena pro pripojeni*/
	public JTextField jmenoTF = new JTextField();
	
	/** Textfield pro nacteni portu pro pripojeni*/
	public JTextField portTF = new JTextField();
	
	/** List model pro zobrazeni her*/
	public DefaultListModel<Hra> hryModel;
	
	/**JList pro zobrazeni her*/
	private JList<Hra> hry;
	
	/** Tlacitko nova hra */
	private JButton novaBT;
	
	/** Tlacitko pripoijt*/
	private JButton pripojBT;
	
	/** Klient pro spravu pripojeni */
	private clientTCP client;
	
	/** Hlavni okno */
	private JPanel windowPN;
	
	/** Panel s tlacitky */
	private JPanel tlacitkaPN;
	
	/** Label s informacemi*/
	private JLabel infoLB;

	/** Polozka menu nova hra */
	private JMenuItem novaHra;
	

	/*********************
	 * Vytvori hlavni okno aplikace
	 */
	public HlavniPanel() 
	{
		this.setTitle("Clovece");
		this.getContentPane().add(vytvorOkno());
		
		this.setSize(500, 500);
		this.setMaximumSize(new Dimension(800,800));
		this.setMinimumSize(new Dimension(150,150));
		
		this.setLocationRelativeTo(null);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setVisible(false);
	}
	
	/**
	 * Zobrazi dialog pro pripojeni
	 */
	public void pripojDialog()
	{

		JComponent[] inputs = {new JLabel("Jmeno"), jmenoTF, new JLabel("IP adresa serveru"), adresaTF, new JLabel("Port"), portTF};
		
		if(JOptionPane.showConfirmDialog(null, inputs, "Pripojeni k serveru", JOptionPane.OK_CANCEL_OPTION) == 2)
			System.exit(0);
	}
	
	/*public void barvyDialog()
	{
		final JButton redBT = new JButton("  ");
		final JButton blueBT = new JButton("  ");
		final JButton greenBT = new JButton("  ");
		final JButton yellBT = new JButton("  ");
		
		greenBT.setBackground(Color.GREEN);
		redBT.setBackground(Color.RED);
		yellBT.setBackground(Color.YELLOW);
		blueBT.setBackground(Color.BLUE);
		
		//greenBT.addActionListener();
		ActionListener listener = new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				JButton source = (JButton)e.getSource();
				Window w = SwingUtilities.getWindowAncestor(source);
				
				if(source.equals(greenBT))
					client.odesli("barva;zel");
				if(source.equals(blueBT))
					client.odesli("barva;mod");
				if(source.equals(redBT))
					client.odesli("barva;cer");
				if(source.equals(yellBT))
					client.odesli("barva;zlu");
				
			    if (w != null) {
			      w.setVisible(false);
			    }
			}
		};	
		
		redBT.addActionListener(listener);
		blueBT.addActionListener(listener);
		yellBT.addActionListener(listener);
		greenBT.addActionListener(listener);
		
		redBT.setEnabled(!deska.cerveny);
		greenBT.setEnabled(!deska.zeleny);
		yellBT.setEnabled(!deska.zluty);
		blueBT.setEnabled(!deska.modry);
		
		
		Object[] options = {redBT, blueBT, greenBT, yellBT};
		JOptionPane.showOptionDialog(null,"Vyberte barvu vasich figurek","Vyber barvy",	JOptionPane.YES_NO_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE,	null,options,null);
	}
	*/
	
	/**
	 * Zobrazi dialog se zpravou o vyhre
	 * @param zprava zprava obsahujici informace, kdo vyhral
	 */
	public void vyhraDialog(String zprava)
	{

			Object [] volby = {"Hrat dalsi", "Vypnout hru"};			
			
			int res = JOptionPane.showOptionDialog(this, zprava, "Hrat znovu?", JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE, null, volby,volby[0]);
			
			if(res == 0)
				client.odesli("odpoj");
			else client.odesli("konec");
	}
	
	/**
	 * Zobrazi dialog se zpravou a barve klienta
	 */
	public void barvaDialog()
	{
			String zprava = "Vase barva je ";
			
			if(deska.getMe().getBarva().equals(Color.RED))
				zprava += "cervena";
			if(deska.getMe().getBarva().equals(Color.BLUE))
				zprava += "modra";
			if(deska.getMe().getBarva().equals(Color.YELLOW))
				zprava += "zluta";
			if(deska.getMe().getBarva().equals(Color.GREEN))
				zprava += "zelena";
			
			JOptionPane.showMessageDialog(this, zprava, "Vase barva", JOptionPane.INFORMATION_MESSAGE);
	}
	
	/**
	 * Zmeni vzhled okna
	 * Odebere list s vypisem her a tlacitka
	 * Prida menu
	 * Zobrazi hraci desku
	 * Prida label s informacemi
	 */
	public void zapniHru()
	{
		setVisible(false);
		setJMenuBar(vytvorMenu());
		setSize(750, 730);
		setTitle("Clovece (hra -" + client.getHraID()+")");
		
		windowPN.remove(hry);
		windowPN.remove(tlacitkaPN);
		windowPN.add(deska, BorderLayout.CENTER);
		
		deska.setVisible(true);
		setVisible(true);
		
	}
	
	/**
	 * Vrati okno zpet do puvodniho stavu
	 * Odebere desku
	 * Zobrazi seznam her a tlacitka
	 * Odebere menu
	 */
	public void vypniHru()
	{
		setVisible(false);
		setJMenuBar(vytvorMenu());
		setTitle("Clovece");
		setSize(500, 500);

		windowPN.remove(deska);
		windowPN.add(tlacitkaPN, BorderLayout.NORTH);
		windowPN.add(hry, BorderLayout.CENTER);
		
		infoLB.setText("");
		infoLB.setBackground(Color.WHITE);
		
		setJMenuBar(null);
		setVisible(true);
	}
	
	/**
	 * Zmeni informaci labelu podle zadaneho kodu
	 * @param arg kod stavu, ktery se ma zobrazit
	 */
	public void cekej(int arg)
	{
		switch(arg)
		{
		case 1: 
		{
			infoLB.setText("Hru začnete kliknutim na polozku 'Zacit' v menu Hra");
			novaHra.setEnabled(true);
		}
		break;

		case 2: {
			infoLB.setText("Cekani na dalsi hrace");
			novaHra.setEnabled(false);
		}
		break;


		case 3:{
			infoLB.setText("Prosim cekejte, nez hra zacne");
			novaHra.setEnabled(false);
		}
		break;
		

		}
	}
	
	/**
	 * Vytvori menu hra
	 * @return instanci menu
	 */
	public JMenuBar vytvorMenu()
	{
		JMenuBar menu = new JMenuBar();
		JMenu hra = new JMenu("Hra");
		
		novaHra = new JMenuItem("Zacit");
		novaHra.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				client.odesli("zacni_hru");
				
			}
		});
		novaHra.setEnabled(false);
		
		JMenuItem odpojit = new JMenuItem("Odpojit");
		odpojit.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				client.odesli("odpoj");
				
			}
		});
		
		JMenuItem vypnout = new JMenuItem("Vypnout");
		vypnout.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				client.odesli("konec");	
			}
		});
		
		hra.add(novaHra);
		hra.add(odpojit);
		hra.addSeparator();
		hra.add(vypnout);
		menu.add(hra);
		return menu;
	}

	/**
	 * Vytvori hlavni okno
	 * @return instance hlavniho panelu
	 */
	public Component vytvorOkno()
	{

		windowPN = new JPanel(new BorderLayout());
		deska = new Deska(this);
		hryModel = new DefaultListModel<Hra>();
		
		hry = new JList<Hra>(hryModel);
		hry.addMouseListener(new MouseAdapter() {
		    public void mouseClicked(MouseEvent evt) {
		        if (evt.getClickCount() == 2) {
		            client.odesli("pripoj;"+hry.getSelectedValue().id);
		        }
		    }
		});
		windowPN.add(hry, BorderLayout.CENTER);
		windowPN.add(vytvorTlacitkaPN(), BorderLayout.NORTH);
		
		infoLB = new JLabel("", SwingConstants.CENTER);
		infoLB.setFont(new Font("Serif", Font.PLAIN, 18));
		infoLB.setOpaque(true);
		
		windowPN.add(infoLB, BorderLayout.SOUTH);
		return windowPN;
	}
	
	/**
	 * Vytvori panel s tlacitky
	 * @return instance panelu s tlacitky
	 */
	public Component vytvorTlacitkaPN()
	{
		tlacitkaPN = new JPanel();
		novaBT = new JButton("Nova hra");
		pripojBT = new JButton("Pripojit");
		
		novaBT.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				client.odesli("nova_hra");
				
			}
		});
		
		pripojBT.addActionListener(new ActionListener() {
			
			@Override
			public void actionPerformed(ActionEvent e) {
				if(hry.getSelectedValue() != null)
				client.odesli("pripoj;"+hry.getSelectedValue().id);
				
			}
		});
		tlacitkaPN.add(novaBT);
		tlacitkaPN.add(pripojBT);
		return tlacitkaPN;

	}
	
	//Gettry a settry
	public JLabel getInfoLB() {
		return infoLB;
	}


	public void setInfoLB(JLabel infoLB) {
		this.infoLB = infoLB;
	}
	
	public Deska getDeska() {
		return deska;
	}


	public clientTCP getClient() {
		return client;
	}
	
	public void setClient(clientTCP client)
	{
		this.client = client;
	}
}