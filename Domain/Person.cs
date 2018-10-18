using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;

namespace Domain
{
    public class Player
    {
        public string Name { get; set; }

        public Color TextColor {get;set;}
        public Color BGColor {get;set;}
        public GameBoard Board;
        public bool AI;

        public Player(string name, GameBoard board)
        {
            Name = name;
            Board = board;
        }
        

        public override string ToString()
        {
            return Name;
        }
    }
}