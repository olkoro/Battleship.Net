using System;

namespace MenuSystem
{
    public class MenuElement
    {
        public string Title { get; set; }
        public Action Method { get; set; }
        public bool GoBackAfter = false;
        public string Value { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public MenuElement()
        {
            
        }
    }
    
}