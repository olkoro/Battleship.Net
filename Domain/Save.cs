using System;
using System.Collections.Generic;

namespace Domain
{
    public class Save
    {
        public int SaveId { get; set; }
        public DateTime Time;
        public List<State> States;
        public string Status = "in progress";
        public bool CanTouch = false;

        public Save(List<State> states)
        {
            States = states;
            Time = DateTime.Now;
        }
        public Save(){}
    }
}