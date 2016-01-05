import java.net.*;
import javax.swing.JOptionPane;

import java.awt.Dialog;
import java.awt.Window;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.*;


class App
{
	public static int port = 10000;
	public static String host = "";

	public static Sender s;

	public static void main(String argv[]) throws Exception
	{
		try{
			String local = "";

			try{ 
				//zkusi ziskat vzorou adresu jako adresu mistniho pocitace
				local = InetAddress.getLocalHost().getHostAddress();
				//local = "127.0.0.1"; //THIS IS ONLY FOR DEVEL VERSION, IN PRODUCTION VERSION MAY BE DELETED!!
			}catch(UnknownHostException e){
				//pokud to nejde, vlozi adresu localhost
				local = "127.0.0.1";
			}finally {	
				//nakonec vzdy prida jeste predem definovany port
				local += ":" + port;
			}

			//ziskani IP adresy a portu z uzivatelskeho vstupu
			host = (String) JOptionPane.showInputDialog(null, "IP: ", "Info", JOptionPane.INFORMATION_MESSAGE, null, null, local);

			//nastaveni promennych na zadane hodnoty
			port = Integer.parseInt(host.substring(host.indexOf(":") + 1));
			host = host.substring(0, host.indexOf(":"));

			//pokus o vytvoreni noveho pripojeni
			InetAddress adresa = InetAddress.getByName(host); //umozni zadani serveru i jeho jmenem
			Socket socket = new Socket(adresa.getHostAddress(), port); //vytvori novy socket se zadanymi parametry

			System.out.print("Pripojuju se na : "+adresa.getHostAddress()+" se jmenem : "+adresa.getHostName()+"\n" ); /* vypise ke komu jsme sep ripojili */
			System.out.println("Cekam na pripojeni druheho hrace...");

			// korektrne ukonci druheho klienta pokud stikneme CTRL+C v konzoli
			s = new Sender(socket);
			Runtime.getRuntime().addShutdownHook(new Thread(){ public void run(){ s.sendMsg("kill;"); } });

			//vytvoreni vlakna pro prijimani
			Primani primani = new Primani(socket);
			primani.start();
		}catch(Exception e){
			System.err.println("Error: "+e.getMessage());
			JOptionPane.showMessageDialog(null, "Error: "+e.getMessage(), "Alert", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}
	}

}
