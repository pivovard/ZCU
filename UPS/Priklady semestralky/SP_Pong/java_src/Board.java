import java.awt.Color;
import java.awt.Rectangle;

public class Board extends Rectangle {
	
	public final static int WIDTH = 10;
	public final static int HEIGHT = 65;
	public final static char LEFT = 'L';
	public final static char RIGHT = 'R';
	
	private final boolean controllable;
	private final char side;
	
	public Board(int x, int y, char side, boolean controllable){
		super(x, y, WIDTH, HEIGHT);
		this.controllable = controllable;
		this.side = side;
	}
	
	public Board(char side, boolean controllable){
		super(WIDTH, HEIGHT);
		this.controllable = controllable;
		this.side = side;
		this.init();
	}
	
	public void move(int yDirection){
		y += yDirection;
		
		if(y >= 395)
			y = 395;
		else if(y <= 30)
			y = 30;
	}
	
	public void setPos(int yPos){
		y = yPos;
		
		if(y >= 395)
			y = 395;
		else if(y <= 30)
			y = 30;
	}
	
	public void init(){
		if(side == LEFT){
			this.x = 30;
			this.y = (Game.GAME_H - HEIGHT)/2;
		}else {
			this.x = (Game.GAME_W - WIDTH - 30);
			this.y = (Game.GAME_H - HEIGHT)/2;
		}
	}
	
	public Color getColor(){
		if(controllable){
			return Color.CYAN;
		}else {
			return Color.WHITE;
		}
	}

	public char getSide() {
		return side;
	}
	
}
