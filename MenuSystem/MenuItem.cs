using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class MenuItem
    {
        public string ShortcutDescription { get; set; }
        public string LongDescription { get; set; }

        public List<string> Shortcuts { get; set; } = new List<string>();
    
        // parameter is shortcut chosen
        public Func<string> CommandToExecute { get; set; } // 1
        public Func<string, string> CommandToExecute2 { get; set; } // string somemethod(string userChoice)

        public bool IsDefaultChoice { get; set; } = false;
        
        public override string ToString()
        {
            return ShortcutDescription + ") " + LongDescription;
        }
        
    }
}