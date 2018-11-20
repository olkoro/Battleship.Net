using System;
using System.Collections.Generic;

namespace Domain
{
    public class State
    {
        public Player P1;
        public Player P2;
        public bool CanTouch;
        public bool P2Turn;
        public DateTime time;
        public int StateId { get; set; }
        
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