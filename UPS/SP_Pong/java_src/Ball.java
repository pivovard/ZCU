import java.awt.Color;
import java.awt.Rectangle;
import java.util.Random;

public class Ball extends Rectangle{
	
	/**
	 * Serial Version UID
	 */
	private static final long serialVersionUID = 8411L;
	
	private final static int SIZE = 10;
	private final Color color = Color.WHITE;
	
	private static int initX = Game.GAME_W/2 - SIZE/2;
	private static int initY = Game.GAME_H/2 - SIZE/2;
	
	private static Random rnd;
	
	public Ball(){
		super(initX, initY, SIZE, SIZE);
		rnd = new Random();
	}
	
	public void moveBall(int x, int y){
		if(y <= 30)
			this.y = 30;
		else
			this.y = y;
		
		this.x = x;
	}
	
	public void init(){
		this.x = initX;
		this.y = 75 + rnd.nextInt(Game.GAME_H - 150);
	}
	
	public Color getColor(){
		return this.color;
	}
}
