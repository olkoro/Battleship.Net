using System;
using System.Collections.Generic;

namespace Domain
{
    public class Save
    {
        public int SaveId { get; set; }
        public DateTime Time;
        public List<State> States { get; set;}
        public string Status { get; set; }
        public bool CanTouch { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        
        
        

        public Save(List<State> states)
        {
            States = states;
            Time = DateTime.Now;
        }

        public Save()
        {
            Time = DateTime.Now;
        }
    }
}