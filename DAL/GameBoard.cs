using System;
using System.Collections.Generic;
using Domain;

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
        public Domain.GameBoard GetDomainBoard()
        {
            var domainboard = new Domain.GameBoard(rows,cols);
            //var board = new List<List<Domain.BoardSquareState>>(rows);
            foreach (var square in this.Squares)
            {
                domainboard.Board[square.x][square.y] = Domain.GameBoard.Convert(square.Value);
            }

            foreach (var ship in Ships)
            {
                Console.WriteLine("ship");
                domainboard.Ships.Add(ship.GetDomainShip());
            }
            return domainboard;
        }
        
    }
}