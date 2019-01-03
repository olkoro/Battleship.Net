using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.Pages;
using GameBoard = Domain.GameBoard;
using Player = Domain.Player;
using Rules = Domain.Rules;
using Ship = Domain.Ship;

namespace WebApp
{
    public class WebUI
    {
        public static Player Player1 = new Player("Player 1", new GameBoard(Rules.Boardrows, Rules.Boardcolumns),new GameBoard(Rules.Boardrows,Rules.Boardcolumns))
        {
            Ships = new List<Ship>(Rules.Ships)
        };
        public static Player Player2 = new Player("AI", new GameBoard(Rules.Boardrows,Rules.Boardcolumns),new GameBoard(Rules.Boardrows,Rules.Boardcolumns))
        {
            AI = true,Ships = new List<Ship>(Rules.Ships)
        };
        public static bool P2Turn = false;
        public static Player Current = Player1;
        public static Player Other = Player2;
        public static int ShipInHand = 0;//Rules.Ships[0].Length;
        public static bool RotationInHand = false;
        public static void AIPlace()
        {
            AI.AIPlacing(Player2.Ships, Player2.Board);
        }

        public static void Run()
        {
            Player1.Ships = new List<Ship>(Rules.Ships);
            Player2.Ships = new List<Ship>(Rules.Ships);
        }

        public static void LoadSave(int id)
        {
            var ctx = new AppDbContext();
            var save = ctx.Saves.Where(s => s.SaveId.Equals(id))
                .Include(s => s.Player1).Include(s => s.Player2)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player1GB).ThenInclude(g => g.Ships).ThenInclude(s => s.ShipsLocations)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player2GB).ThenInclude(g => g.Ships).ThenInclude(s => s.ShipsLocations)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player1GB).ThenInclude(g => g.Squares)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player2GB).ThenInclude(g => g.Squares)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player1Map).ThenInclude(g => g.Squares)
                .Include(s => s.LastState)
                .ThenInclude(s => s.Player2Map).ThenInclude(g => g.Squares)
                .Include(s => s.Rules)
                .First();
            if (save.Replay)
            {
                var replayquery = ctx.Saves.Where(s => s.SaveId == save.SaveId)
                    .Include(s => s.Player1).Include(s => s.Player2).Include(s=>s.Rules)
                    .Include(s=>s.States).ThenInclude(s=>s.Player1GB).ThenInclude(g => g.Squares)
                    .Include(s=>s.States).ThenInclude(s=>s.Player2GB).ThenInclude(g => g.Squares)
                    .Include(s=>s.States).ThenInclude(s=>s.Player1Map).ThenInclude(g => g.Squares)
                    .Include(s=>s.States).ThenInclude(s=>s.Player2Map).ThenInclude(g => g.Squares)
                    .First();
                foreach (var state in replayquery.States)
                {
                    SaveSystem.GameStates.Add(state.GetDomainState(replayquery.Player1.GetDomainPlayer(state.Player1GB.GetDomainBoard(),state.Player1Map.GetDomainBoard()),
                        replayquery.Player2.GetDomainPlayer(state.Player2GB.GetDomainBoard(),state.Player2Map.GetDomainBoard())
                        ,replayquery.Rules.CanTouch));
                }
            }
            Player player1 = new Domain.Player(save.Player1.Name, 
                save.LastState.Player1GB.GetDomainBoard(),
                save.LastState.Player1Map.GetDomainBoard()){AI = save.Player1.AI};
            
            Player player2 = new Domain.Player(save.Player2.Name, 
                save.LastState.Player2GB.GetDomainBoard(),
                save.LastState.Player2Map.GetDomainBoard()){AI = save.Player2.AI};
            Rules.CanTouch = save.Rules.CanTouch;
            bool p2Turn = save.LastState.P2Turn;
            Player1 = player1;
            Player2 = player2;
            Current = Player1;
            Other = Player2;
            if (p2Turn)
            {
                SwitchSwitch();
            }
            //PlayGame(player1,player2, p2Turn);
        }

        public static void PlaceRandomly()
        {
            Current.Board = new GameBoard(Rules.Boardrows, Rules.Boardcolumns);
            AI.AIPlacing(Current.Ships, Current.Board);
        }

        public static void Switch()
        {
            if (Current == Player1)
            {
                Current = Player2;
                Other = Player1;
            }
            else
            {
                Current = Player1;
                Other = Player2;
            }
        }

        public static string ShootAI()
        {
            var ret = "unknown";
            ret = AI.AIShoot(Player1.Board, Player2.Map);
            if(ret == "MISS "){SwitchSwitch();}
            else
            {
                ShootAI();
            }
            return ret;
        }
        public static string Shoot(string data)
        {
            string[] coords0 = data.Split(",");
            int y = 0;
            if (Int32.TryParse(coords0[0].ToString(), out y))
            {
                int x = 0;
                if (Int32.TryParse(coords0[1].ToString(), out x))
                {
                    int[] coords = new[]{y,x};
                    var ret = Domain.GameBoard.Shoot(Other.Board, coords, Current.Map);
                    if (ret == "MISS ")
                    {
                        SwitchSwitch();
                    }
                    return ret;
                }
            }

            return "Could not parse your input: "+data;
        }

        public static int[] ParseCoords(string data)
        {
            string[] coords0 = data.Split(",");
            int y = 0;
            if (Int32.TryParse(coords0[0].ToString(), out y))
            {
                int x = 0;
                if (Int32.TryParse(coords0[1].ToString(), out x))
                {
                    int[] coords = new[]{y,x};
                    return coords;
                }
            }
            return null;
        }

        public static GameBoard[] GetGameBoards()
        {
            return new []{Current.Board, Current.Map};
        }

        public static string ConvertToSymbols(string s)
        {
            if (s == "Empty")
            {
                return "💧";
            }else if (s == "Ship")
            {
                return "⚓️";}

            if (s == "Dead")
            {
                return "🔥";
            }

            if (s == "Hit")
            {
                return "💥";
            }

            if (s == "Miss")
            {
                return "💦";
            }

            return s;
        }

        public static void SwitchSwitch()
        {
            if (P2Turn)
            {
                P2Turn = false;
            }
            else
            {
                P2Turn = true;
            }
            Switch();
        }

        public static Player GetWinner()
        {
            return AI.GetWinner(Player1,Player2);
        }

        public static void Menu(IApplicationBuilder app)
        {
            app.Run(async (context) => { await context.Response.WriteAsync("1)Start game\n2)Load Game"); });
            
        }

        public static string GetString()
        {
            var gb = GetBoardString(Player1.Board).Split("\n");
            var map = GetBoardString(Player1.Map).Split("\n");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < gb.Length; i++)
            {
                sb.Append(gb[i]);
                sb.Append("     |     ");
                sb.Append(map[i] + "\n");
            }
            return sb.ToString();
        }

        public static void RunSP()
        {
            
        }
        public static string GetBoardString(GameBoard gameBoard)
        {
            var sb = new StringBuilder();
            //sb.Append("    A.  B.  C.  D.  E.  F.  G.  H.  I.  J. \n");
            sb.Append("    ");
            for (int j = 0; j < Rules.Boardcolumns - 1; j++)
            {
                sb.Append(GameBoard.Coordinates[j]);
                sb.Append(".  ");
            }
            sb.Append(GameBoard.Coordinates[Rules.Boardcolumns - 1]);
            sb.Append(". \n");
            int i = 0;

            foreach (var boardRow in gameBoard.Board)
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
            sb.Append(" "+GetRowSeparator(gameBoard.Board.First().Count));
            return sb.ToString();
        }
        public static string GetRowSeparator(int elemCountInRow)
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

        public static string GetRowWithData(List<BoardSquareState> boardRow)
        {
            var sb = new StringBuilder();
            foreach (var boardSquareState in boardRow)
            {
                sb.Append("| " + GetBoardSquareStateSymbol(boardSquareState) + " ");
            }

            sb.Append("|");
            return sb.ToString();
        }

        public static string GetBoardSquareStateSymbol(BoardSquareState state)
        {
            switch (state)
            {
                case BoardSquareState.Empty: return " ";
                case BoardSquareState.Ship: return "□";
                case BoardSquareState.Miss: return "·";
                case BoardSquareState.Hit: return "■";
                case BoardSquareState.Dead: return "F";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}