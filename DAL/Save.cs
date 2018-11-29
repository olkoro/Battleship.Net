using System;
using System.Collections.Generic;

namespace DAL
{
    public class Save
    {
        public int SaveId { get; set; }

        public string TimeStamp { get; set; }
        //public List<State> States  { get; set; }
        
        //FK
        
        public int Player1Id { get; set; }
        public Player Player1 { get; set; }
        
        public int Player2Id { get; set; }
        public Player Player2 { get; set; }
        
        public int RulesId { get; set; }
        public Rules Rules { get; set; }
        
        public int LastStateId { get; set; }
        public State LastState { get; set; }
        
        public List<State> States { get; set; }= new List<State>();

        public Save()
        {
            TimeStamp = DateTime.Now.ToString();
        }

        public override string ToString()
        {
            try
            {
                return Player1.Name + " VS "+ Player2.Name + " " + TimeStamp;
            }
            catch (Exception e)
            {
                return "could not get the name of this save";
            }
        }
    }
}