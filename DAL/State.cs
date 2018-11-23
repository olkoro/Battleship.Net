using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class State
    {
        public int StateId { get; set; }
        public string TimeStamp { get; set; }

        // Convention - FK
        public int Player1GBId { get; set; }  
        public GameBoard Player1GB { get; set; }
        
        public int Player2GBId { get; set; }  
        public GameBoard Player2GB { get; set; }
        
        public int Player1MapId { get; set; }  
        public GameBoard Player1Map { get; set; }
        
        public int Player2MapId { get; set; }  
        public GameBoard Player2Map { get; set; }
        
        [Column(TypeName = "bit")]
        public bool P2Turn { get; set; }

        public State()
        {
            TimeStamp = DateTime.Now.ToString();
        }
        
    }
}