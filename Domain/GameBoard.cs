using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class GameBoard
    {
        public static List<string> Coordinates = new List<string>(new string[]     {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
        });
        public List<List<BoardSquareState>> Board { get; set; } = new List<List<BoardSquareState>>();
        public List<Ship> Ships { get; set; } = new List<Ship>();

        public int ships = 0;

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


        // FIXME: UI methods don't belong in Domain!!!!!

        public string GetBoardString()
        {
            var sb = new StringBuilder();
            //sb.Append("    A.  B.  C.  D.  E.  F.  G.  H.  I.  J. \n");
            sb.Append("    ");
            for (int j = 0; j < Rules.Boardcolumns - 1; j++)
            {
                sb.Append(Coordinates[j]);
                sb.Append(".  ");
            }
            sb.Append(Coordinates[Rules.Boardcolumns - 1]);
            sb.Append(". \n");
            int i = 0;

            foreach (var boardRow in Board)
            {
                sb.Append(" "+GetRowSeparator(boardRow.Count) + "\n");
                sb.Append((i + 1).ToString());
                if((i + 1) < 10)
                {
                    sb.Append(" ");
                }
                i++;
                sb.Append(GetRowWithData(boardRow) + "\n");
            }
            sb.Append(" "+GetRowSeparator(Board[0].Count));
            return sb.ToString();
        }

        public string GetRowSeparator(int elemCountInRow)
        {
            var sb = new StringBuilder();
            sb.Append(" ");
            for (int i = 0; i < elemCountInRow; i++)
            {
                sb.Append("+---");
            }

            sb.Append("+");
            return sb.ToString();
        }

        public string GetRowWithData(List<BoardSquareState> boardRow)
        {
            var sb = new StringBuilder();
            foreach (var boardSquareState in boardRow)
            {
                sb.Append("| " + GetBoardSquareStateSymbol(boardSquareState) + " ");
            }

            sb.Append("|");
            return sb.ToString();
        }

        public string GetBoardSquareStateSymbol(BoardSquareState state)
        {
            switch (state)
            {
                case BoardSquareState.Empty: return " ";
                case BoardSquareState.Ship: return "+";
                case BoardSquareState.Miss: return "*";
                case BoardSquareState.Hit: return "x";
                case BoardSquareState.Dead: return "F";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
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

            return clone;
        }
    }
}