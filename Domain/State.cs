using System;
using System.Collections.Generic;

namespace Domain
{
    public class State
    {
        public int Turn;
        public Player P1;
        public Player P2;
        public GameBoard P1GameBoard;
        public GameBoard P2GameBoard;
        public GameBoard P1Map;
        public GameBoard P2Map;
        public List<Ship> P1Ships;
        public List<Ship> P2Ships;
        public bool CanTouch;
        public bool P2Turn;
        public DateTime time;
        public int StateID { get; set; }
        
        public State(){}

        public State(Player p1,Player p2, bool canTouch, bool P2turn)
        {
            P1 = p1;
            P2 = p2;
            CanTouch = canTouch;
            P2Turn = P2turn;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return time + " " + P1 + " VS " +P2;
        }
    }
}