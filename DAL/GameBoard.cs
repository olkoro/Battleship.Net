using System.Collections.Generic;

namespace DAL
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        
        public int rows { get; set; }
        public int cols { get; set; }
        
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<GameboardSquare> Squares { get; set; }= new List<GameboardSquare>();
        
//        public int BoardShipsID { get; set; }
//        public List<Ship> BoardShips { get; set; }
    }
}