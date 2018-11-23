using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class GameboardSquare
    {
        public int GameboardSquareId { get; set; }
        
        [Column(TypeName = "int")]
        public int x { get; set; }
        
        [Column(TypeName = "int")]
        public int y { get; set; }
        
        public string Value { get; set; }
        
        //FK
        public int GameBoardId { get; set; }
        public GameBoard GameBoard { get; set; }
        
        
        

    }
}