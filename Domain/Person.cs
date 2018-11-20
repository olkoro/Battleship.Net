using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;

namespace Domain
{
    public class Player
    {
        public string Name { get; set; }
        public int PlayerId { get; set; }

        public GameBoard Board;
        public GameBoard Map;
        public bool AI;
        public List<Ship> Ships { get; set; } = new List<Ship>();

        public Player(string name, GameBoard board, GameBoard map)
        {
            Name = name;
            Board = board;
            Map = map;
        }

        public Player(Player player)
        {
            Name = player.Name;
            Board = GameBoard.CloneBoard(player.Board);
            Map = GameBoard.CloneBoard(player.Map);
            Ships = new List<Ship>(player.Ships);
            AI = player.AI;
        }
        

        public override string ToString()
        {
            return Name;
        }
    }
}