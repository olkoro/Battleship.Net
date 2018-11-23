namespace DAL
{
    public class BoardShips
    {
        public int BoardShipsId { get; set; }
        
        //FK
        public int GameBoardId { get; set; }
        public GameBoard GameBoard { get; set; }
        
        public int ShipId { get; set; }
        public Ship Ship { get; set; }
    }
}