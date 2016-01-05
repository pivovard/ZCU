import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;
import java.net.*;

//import pong.Board;

import javax.swing.JFrame;

public class Game extends JFrame {
	
	/**
	 * Serial Version UID
	 */
	private static final long serialVersionUID = 1L;
	public static int GAME_W;// = 800;
	public static int GAME_H;// = 450;
	
	int x, y, ballX, ballY, score1 = 0, score2 = 0;
	char myBoardPos;
	private Image dBufImage;
	private Graphics dBufGraphics;

	Font font = new Font("Consolas", Font.PLAIN, 35);
	
	Ball ball = new Ball();
	Board b1, b2, lboard, rboard; 
	
	int goalZone1, goalZone2;
	
	Sender sender;
	
	
	
	public Game(char player, Socket socket, int width, int height){
		GAME_W = width;
		GAME_H = height;
		myBoardPos = player;
		sender = new Sender(socket);
		addWindowListener(new ALWindow());
		
		if(player == 'L'){
			lboard = b1 = new Board(Board.LEFT, true);
			rboard = b2 = new Board(Board.RIGHT, false);
		}else {
			rboard = b1 = new Board(Board.RIGHT, true);
			lboard = b2 = new Board(Board.LEFT, false);
		}
		
		//for collision detection
		goalZone1 = lboard.x+lboard.width;
		goalZone2 = rboard.x;	
		
		addKeyListener(new ALKey());
		addMouseMotionListener(new ALMouse());
		setTitle("Pong");
		setSize(GAME_W, GAME_H);
		setResizable(false);
		setVisible(true);
		setBackground(Color.BLACK);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	
	public void paint(Graphics g){
		dBufImage = createImage(GAME_W, GAME_H);
		dBufGraphics = dBufImage.getGraphics();
		paintComponent(dBufGraphics);
		g.drawImage(dBufImage, 0, 0, this);
	}
	
	public void paintComponent(Graphics g){
		//score
		g.setFont(font);
		g.setColor(Color.MAGENTA);
		g.drawString(score1+":"+score2, 372, 55);
		
		//middle line
		/*g.setColor(Color.WHITE);
		g.drawLine(GAME_W/2, 0, GAME_W/2, GAME_H);
		g.drawLine(0, GAME_H/2, GAME_W, GAME_H/2);/**/
		
		//ball
		g.setColor(ball.getColor());
		g.fillRect(ball.x, ball.y, ball.width, ball.height);
		
		//fill and collision BALL with LEFT_BOARD
		if(b1.intersects(ball)){
			g.setColor(Color.ORANGE);
		}else {
			g.setColor(b1.getColor());
		}
		g.fillRect(b1.x, b1.y, b1.width, b1.height);
		
		//fill and collision BALL with RIGHT_BOARD
		if(b2.intersects(ball)){
			g.setColor(Color.ORANGE);
		}else {
			g.setColor(b2.getColor());
		}
		g.fillRect(b2.x, b2.y, b2.width, b2.height);
		
		
		//goal line 1
		if(ball.x+2 <= goalZone1){
			//goal to Player1
			g.setColor(Color.RED);
			ball.init();
		} else {
			g.setColor(Color.GRAY);
		}
		g.drawLine(lboard.x+lboard.WIDTH, 0, lboard.x+lboard.WIDTH, GAME_H);
		
		//goal line 2
		if((ball.x+ball.width-2) >= goalZone2){
			//goal to Player2
			g.setColor(Color.RED);
			ball.init();
		} else {
			g.setColor(Color.GRAY);
		}
		g.drawLine(rboard.x, 0, rboard.x, GAME_H);
		
		try {
			Thread.sleep(1);
		}catch(Exception e){}
		
		repaint();
	}

	/*public static void main(String[] args) {
		new Game('R');
	}*/
	
	public void setBall(int x, int y){
		ball.moveBall(x-12, y-12);
	}
	
	public void setScore(int s1, int s2){
		this.score1 = s1;
		this.score2 = s2;
	}
	
	
	public class ALKey extends KeyAdapter {
		public void keyPressed(KeyEvent e){
			int key = e.getKeyCode();
			if(key == KeyEvent.VK_UP){
				//b1.move(-5);
				sender.sendMsg("move;up;");
			}
			if(key == KeyEvent.VK_DOWN){
				//b1.move(5);
				sender.sendMsg("move;down;");
			}
		}
		public void keyReleased(KeyEvent e){}
	}
	
	public class ALMouse extends MouseAdapter {
		@Override
		public void mouseDragged(MouseEvent e){
			ball.moveBall(e.getX() - 12, e.getY() - 12);
		}
	}
	
	public class ALWindow implements WindowListener {
		public void windowOpened(WindowEvent arg0) {}
		public void windowIconified(WindowEvent arg0) {}
		public void windowDeiconified(WindowEvent arg0) {}
		public void windowDeactivated(WindowEvent arg0) {}
		public void windowClosed(WindowEvent arg0) {}
		public void windowActivated(WindowEvent arg0) {}
		
		@Override
		public void windowClosing(WindowEvent arg0) {
			try{
				sender.sendMsg("end;");
			}
			catch(Exception e){
				e.printStackTrace();
			}
		}
	}
}
