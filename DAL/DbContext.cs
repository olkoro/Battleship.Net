using System;
using System.Collections.Generic;
using Domain;

namespace DAL
{
    public class DbContext
    {
        public List<Player> Persons { get; set; } = new List<Player>();
        
    }
}