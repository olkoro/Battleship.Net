using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class GameBoard
    {
        public int GameBoardId { get; set; }
        public static List<string> Coordinates = new List<string>(new string[]     {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","R","S","T","Q",
            "R","S","T","U","V","W","X","Y","Z"
        });
        public List<List<BoardSquareState>> Board { get; set; } = new List<List<BoardSquareState>>();
        public List<Ship> Ships { get; set; } = new List<Ship>();

        public GameBoard(int boardRows, int boardCols)
        {
            for (int i = 0; i < boardRows; i++)
            {
                Board.Add(new List<BoardSquareState>());
                for (int j = 0; j < boardCols; j++)
                {
                    Board[i].Add(BoardSquareState.Empty);
                }
            }
        }
        public GameBoard(){}


        public static string Shoot(GameBoard board, int[] coords,GameBoard map)
        {
            var row = coords[0];
            var column = coords[1];
            if (board.Board[row][column] == BoardSquareState.Empty)
            {
                map.Board[row][column] = BoardSquareState.Miss;
                board.Board[row][column] = BoardSquareState.Miss;
                return "MISS ";
            }
            else
            {
                var ship = AI.FindShip(board, coords);
                ship.SetHealth(ship.Health - 1);
                board.Board[row][column] = BoardSquareState.Hit;
                map.Board[row][column] = BoardSquareState.Hit;
                if (AI.CheckDead(board,coords,map) == true)
                {
                    return "KILL ";
                }
                else
                {
                    return "HIT";
                }
            }
            
            return "";
        }
        public static GameBoard CloneBoard(GameBoard original)
        {
            GameBoard clone = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            for (int i = 0; i < clone.Board.Count; i++)
            {
                for (int j = 0; j < clone.Board[i].Count; j++)
                {
                    clone.Board[i][j] = original.Board[i][j];
                }
            }
            clone.Ships = new List<Ship>(original.Ships);

            return clone;
        }
        public static BoardSquareState Convert(string value){
            if (value == "Miss")
            {
                return BoardSquareState.Miss;
            }

            if (value == "Hit")
            {
                return BoardSquareState.Hit;
            }

            if (value == "Dead")
            {
                return BoardSquareState.Dead;
            }

            if (value == "Ship")
            {
                return BoardSquareState.Ship;
            }
            else
            {
                return BoardSquareState.Empty;
            }
        }
    }
}
