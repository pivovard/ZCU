import java.io.*;
import java.net.*;
import java.lang.Thread;
import javax.swing.JOptionPane;


public class Primani extends Thread{

	/**
	 * Odkaz na socket zajistujici spojeni se serverem
	 */
	public Socket socket;
	/**
	 * Odkaz na hru umoznujici pristup k micku a palkam
	 */
	public Game game;
	/**
	 * Reader pro prijem zprav ze serveru
	 */
	BufferedReader bfr;

	/**
	 * Konstruktor tridy pro prijimani zprav ze serveru
	 * @param <strong>Socket s</strong> - reference na socket
	 * @param <strong>Game g</strong> - reference na hru
	 */
	public Primani(Socket s)
	{
		this.socket = s;
	}

	public void run() {
		MyDialog waitDialog = new MyDialog("Waiting","Cekam na pripojeni druheho hrace...", 60);
		try{
			char pole[] = new char[255];
			bfr = new BufferedReader(new InputStreamReader(socket.getInputStream()));


			String message;
			String command[] = null;

			while(true){  
				if(bfr.read(pole) == -1){
					String msg = "Ztrata spojeni se serverem!";
					System.out.println(msg);
					waitDialog.close();
					JOptionPane.showMessageDialog(null, msg, "Server disconect", JOptionPane.ERROR_MESSAGE);
					System.exit(1);
				}
				
				message = new String(pole);
				//System.out.println("Prijato od serveru: " +  message);
				message = message.replaceAll("\0","");
				command = message.split(";");
				
				if(command[0].equalsIgnoreCase("init")){
					waitDialog.close();
					if(command[1].equalsIgnoreCase("L")){
						game = new Game('L', socket, Integer.parseInt(command[4]), Integer.parseInt(command[5]));
					}else{
						game = new Game('R', socket, Integer.parseInt(command[4]), Integer.parseInt(command[5]));
					}
				}
				else if(command[0].equalsIgnoreCase("coord")){
					//System.out.println("souradnice: "+command[1]+ "," +command[2]);
					game.setBall(Integer.parseInt(command[1]),Integer.parseInt(command[2]));
					/* Levy hrac je na serveru veden jako player 1 !!! */
					if(game.myBoardPos == 'L'){
						game.b1.setPos(Integer.parseInt(command[3]));
						game.b2.setPos(Integer.parseInt(command[4]));
						game.setScore(Integer.parseInt(command[5]),Integer.parseInt(command[6]));
					}else {
						game.b1.setPos(Integer.parseInt(command[4]));
						game.b2.setPos(Integer.parseInt(command[3]));
						game.setScore(Integer.parseInt(command[5]),Integer.parseInt(command[6]));
					}
				}
				else if(command[0].equalsIgnoreCase("end")){
					JOptionPane.showMessageDialog(null, "You WIN :-)", "Information", JOptionPane.INFORMATION_MESSAGE);
					game.dispose();
					System.exit(1);
				}
				else if(command[0].equalsIgnoreCase("fail")){
					waitDialog.close();
					JOptionPane.showMessageDialog(null, "Zadny souper se nepripojil", "Information", JOptionPane.INFORMATION_MESSAGE);
					game.dispose();
					System.exit(1);
				}
				else if(command[0].equalsIgnoreCase("gameover")){
					if(command[1].equalsIgnoreCase("win")){
						JOptionPane.showMessageDialog(null, "You are WINNER!", "Information", JOptionPane.INFORMATION_MESSAGE);
						game.dispose();
						System.exit(1);
					}else if(command[1].equalsIgnoreCase("lose")){
						JOptionPane.showMessageDialog(null, "Sorry, you lose...", "Information", JOptionPane.INFORMATION_MESSAGE);
						game.dispose();
						System.exit(1);
					}
				}
				
				for(int j=0; j<pole.length; j++){
					pole[j] = 0;
				}
			}							
		}
		catch(Exception e){
			e.printStackTrace();
		}
	}
}