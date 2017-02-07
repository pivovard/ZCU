using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace UPS_Scrabble_client
{
    public class Player
    {
        public int ID;
        public string nick;

        public int score = 0;

        /// <summary>
        /// Create new player
        /// </summary>
        /// <param name="pl"></param>
        public Player(string pl)
        {
            string[] split = pl.Split(',');

            ID = int.Parse(split[0]);
            nick = split[1];
        }

        /// <summary>
        /// Recreate existing player including score
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="r"></param>
        public Player(string pl, bool r)
        {
            string[] split = pl.Split(',');

            ID = int.Parse(split[0]);
            nick = split[1];
            score = int.Parse(split[2]);
        }
    }
}
