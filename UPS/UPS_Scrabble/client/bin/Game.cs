using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace UPS_Scrabble_client
{
    public class Game
    {
        public int ID;
        public int N;

        //List of players
        public Player[] Players;
        //This player
        public Player Player;

        public char[][] field;
        public char[] stack;

        public string turn = "";

        private string vocals = "AEIOUY";
        private int[] multiplier = { 1, 3, 3, 2, 1, 3, 3, 3, 1, 2, 2, 2, 2, 2, 1, 3, 10, 2, 2, 3, 1, 3, 10, 10, 1, 3 };


        /// <summary>
        /// Create new game.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pl"></param>
        /// <param name="nick"></param>
        /// <param name="n"></param>
        public Game(string id, string pl, string nick, int n)
        {
            ID = int.Parse(id);
            N = n;

            //init hracu
            string[] pls = pl.Split(';');
            Players = new Player[pls.Count()];

            for(int i = 0; i < pls.Count(); i++)
            {
                Players[i] = new Player(pls[i]);
            }

            //mistni hrac
            Player = Players.Where(p => p.nick == nick).First(); ;

            field = new char[15][];
            stack = new char[10];

            for (int i = 0; i < 15; i++)
            {
                field[i] = new char[15];
                for(int j = 0; j < 15; j++)
                {
                    field[i][j] = '\0';
                }
            }
        }

        /// <summary>
        /// Recreate existing game.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pl"></param>
        /// <param name="turn"></param>
        /// <param name="nick"></param>
        /// <param name="n"></param>
        public Game(string id, string pl, string turn, string nick, int n)
        {
            ID = int.Parse(id);
            N = n;

            //init hracu
            string[] pls = pl.Split(';');
            Players = new Player[pls.Count()];

            for (int i = 0; i < pls.Count(); i++)
            {
                Players[i] = new Player(pls[i], true);
            }

            //mistni hrac
            Player = Players.Where(p => p.nick == nick).First(); ;

            field = new char[15][];
            stack = new char[10];

            for (int i = 0; i < 15; i++)
            {
                field[i] = new char[15];
                for (int j = 0; j < 15; j++)
                {
                    field[i][j] = '\0';
                }
            }

            int x;
            int y;
            string[] c;
            string[] t = turn.Split(';');
            //jednotlive tahy: x,y,char
            for (int i = 0; i < t.Count(); i++)
            {
                c = t[i].Split(',');

                x = int.Parse(c[0]);
                y = int.Parse(c[1]);

                field[x][y] = c[2].ElementAt(0);

                //Program.FormGame.Field_DataGridView.Rows[x].Cells[y].Value = c[2].ElementAt(0);
                //Program.FormGame.Field_DataGridView.Rows[x].Cells[y].Style.BackColor = System.Drawing.Color.Khaki;
            }
        }

        /// <summary>
        /// Generate new chars to the stack
        /// </summary>
        public void Random()
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                if (r.Next() % 2 == 0) stack[i] = (char)(65 + r.Next(26));
                else stack[i] = vocals.ElementAt(r.Next(6));
            }

            Program.FormGame.Stack_DataGridView.Rows.Clear();
            Program.FormGame.Stack_DataGridView.Rows.Add(stack[0], stack[1], stack[2], stack[3], stack[4], stack[5], stack[6], stack[7], stack[8], stack[9]);
        }

        /// <summary>
        /// Multiple poins by char value and by position on field
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Points(char c, int x, int y)
        {
            int i = c - 65;
            int s = multiplier[i];

            // 4x
            if ((x == 0 || x == 14) && (y == 0 || y == 14)) s *= 4;
            // 3x
            if ((x == 0 || x == 14) && (y == 7)) s *= 3;
            if ((x == 7) && (y == 0 || y == 14)) s *= 3;
            // 2x
            if ((x == 4 || x == 10) && (y == 4 || y == 10)) s *= 2;

            return s;
        }

        /// <summary>
        /// Recv turn of other players from the server
        /// </summary>
        /// <param name="player"></param>
        /// <param name="turn"></param>
        public void RecvTurn(string player, string turn)
        {
            int id = int.Parse(player);
            Player pl = Players.Where(p => p.ID == id).First();

            //rozdeleni na tahy - [0] je score
            string[] t = turn.Split(';');
            pl.score = int.Parse(t[0]);
            Program.FormGame.UpdateScore();

            int x;
            int y;
            string[] c;

            //jednotlive tahy: x,y,char
            for(int i = 1; i < t.Count(); i++)
            {
                c = t[i].Split(',');

                x = int.Parse(c[0]);
                y = int.Parse(c[1]);

                field[x][y] = c[2].ElementAt(0);

                Program.FormGame.Field_DataGridView.Rows[x].Cells[y].Value = c[2].ElementAt(0);
                Program.FormGame.Field_DataGridView.Rows[x].Cells[y].Style.BackColor = System.Drawing.Color.Khaki;
            }
        }

        /// <summary>
        /// Actualize Game Form (after reconnect)
        /// </summary>
        public void ActualizeGameFrom()
        {
            for(int i = 0; i < 15; i++)
            {
                for(int j = 0; j < 15; j++)
                {
                    if(field[i][j] != '\0')
                    {
                        Program.FormGame.Field_DataGridView.Rows[i].Cells[j].Value = field[i][j];
                        Program.FormGame.Field_DataGridView.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Khaki;
                    }
                }
            }
        }

    }
}
