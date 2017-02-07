import java.io.*;
import java.net.*;
import java.util.*;


public class Sender {

	/**
	 * Odkaz na socket zajistujici spojeni se serverem
	 */
	public Socket socket;
	
	/**
	 * Writer pro odesilani zprav na server
	 */
	public OutputStreamWriter wr;
	
	/**
	 * Konstruktor tridy pro odeselani zprav na server
	 * @param <strong>Socket s</strong> - reference na socket
	 */
	public Sender(Socket s)
	{
		this.socket = s;
	}

	/**
	 * Metoda pro poslani zpravy na server
	 * @param <strong>String msg</strong> - retezec ktery bude odeslan
	 */
	public void sendMsg(String msg)
	{
		try {
 			wr = new OutputStreamWriter(socket.getOutputStream());
			wr.write(msg);
			//System.out.println("Odeslano: " + msg);
			wr.flush();
		}
		catch (IOException e){
			e.printStackTrace();
		}
	}
}