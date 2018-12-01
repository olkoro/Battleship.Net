using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using Domain;
using Initializers;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using GameBoard = Domain.GameBoard;
using Player = Domain.Player;
using Rules = Domain.Rules;
using Save = DAL.Save;
using Ship = Domain.Ship;
using State = Domain.State;

namespace GameUI
{
    public static class BattleUI
    {   
        public static bool Abort;
        
        public static void RunGame()
        {
            Abort = false;
            var Board1 = new GameBoard(Rules.Boardrows, Rules.Boardcolumns);
            var Board2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Player1 = new Player("Player 1", Board1,Map1);
            var Player2 = new Player("AI", Board2,Map2);
            Player2.AI = true;
            Console.Clear();
            SetUpGame(Player1,Player2);
            PlayGame(Player1,Player2);
        }
        
        public static void RunPvPGame()
        {
            Abort = false;
            var Board1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Board2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Player1 = new Player("Player 1", Board1,Map1);
            var Player2 = new Player("Player 2", Board2,Map2);
            Console.Clear();
            SetUpGame(Player1,Player2);
            PlayGame(Player1,Player2);
        }

        public static void SetUpGame(Player Player1,Player Player2)
        {
            //player arranging
            if (!Player1.AI)
            {
                Placing(Player1, Player1.Board);
            }
            else
            {
                if (AI.AIPlacing(new List<Ship>(Rules.Ships), Player1.Board) == false)
                {
                    Console.WriteLine("The AI could not place all the ships with your rules. You won, congratulations");
                    switch (Console.ReadKey(true).Key)
                    {    
                        case ConsoleKey.Enter:
                            break;
                    }

                    Abort = true;
                }
            }

            if (!Player2.AI)
            {
                Placing(Player2,Player2.Board);
            }
            else
            {
                if (AI.AIPlacing(new List<Ship>(Rules.Ships), Player2.Board) == false)
                {
                    Console.WriteLine("The AI could not place all the ships with your rules. You won, congratulations");
                    switch (Console.ReadKey(true).Key)
                    {    
                        case ConsoleKey.Enter:
                            break;
                    }

                    Abort = true;
                }
            }
        }
        public static void PlayGame(Player Player1,Player Player2, bool P2turn = false)
        {
            //game starts
            Abort = false;
            var coords = new int[2]{0,0};
            var status = "|";
            while (Abort == false)//cycle
            {
                if (!P2turn)
                {
                    coords = Target(GetBoardString(Player1.Map), Player1,coords, true);
                    if (Player1.Map.Board[coords[0]][coords[1]] != BoardSquareState.Empty || Abort)
                    {
                        continue;
                    }
                    status = GameBoard.Shoot(Player2.Board, coords, Player1.Map);
                    Draw(Player1,GetBoardString(Player1.Board), GetBoardString(Player1.Map), status);
                    if (status == "MISS ")
                    {
                        P2turn = true;
                        if (!Player2.AI)
                        {
                            coords[0] = 0;
                            coords[1] = 0;
                        }
                        
                    }
                }
                else
                {
                    if (Player2.AI)
                    {
                        FullscreenMessage("The AI is making a move.");
                        status = AI.AIShoot(Player1.Board, Player2.Map);
                        Draw(Player1,GetBoardString(Player1.Board), GetBoardString(Player1.Map), status);
                        if (status == "MISS ")
                        {
                            P2turn = false;
                        }
                    }
                    else
                    {
                        Draw(Player2, GetBoardString(Player2.Board), GetBoardString(Player2.Map));
                        coords = Target(GetBoardString(Player2.Map), Player2, coords, true);
                        if (Player2.Map.Board[coords[0]][coords[1]] != BoardSquareState.Empty || Abort)
                        {
                            continue;
                        }

                        status = GameBoard.Shoot(Player1.Board, coords, Player2.Map);
                        Draw(Player2, GetBoardString(Player2.Board), GetBoardString(Player2.Map), status);
                        if (status == "MISS ")
                        {
                            P2turn = false;
                            coords[0] = 0;
                            coords[1] = 0;
                        }
                    }
                }
                //check win                
                if (AI.GetWinner(Player1,Player2) != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(AI.GetWinner(Player1,Player2));
                    sb.Append(" won!");
                    var wstate = new State(new Player(Player1),new Player(Player2),Rules.CanTouch, P2turn);
                    wstate.Status = "[Finished: "+sb.ToString()+"]";
                    SaveSystem.GameStates.Add(wstate);
                    FullscreenMessage(sb.ToString());
                    break;
                }
                Console.WriteLine("PRESS ENY KEY TO CONTINUE");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        break;
                }

                if (status == "MISS " && !Player1.AI && !Player2.AI)
                {
                    FullscreenMessage("PASS THE PC TO THE NEXT PLAYER");
                }

                var state = new State(new Player(Player1),new Player(Player2),Rules.CanTouch, P2turn);
                SaveSystem.GameStates.Add(state);
            }

            if (SaveSystem.GameStates.Count != 0)
            {
                if (SaveSystem.GameStates.Last().Status == null)
                {
                    SaveSystem.GameStates.Last().Status = "[" + SaveSystem.GameStates.Last().P1.ToString() +
                                                          ": " + SaveSystem.GameStates.Last().P1.Board.Ships.Count +
                                                          " Ships, " +
                                                          SaveSystem.GameStates.Last().P2.ToString() +
                                                          ": " + SaveSystem.GameStates.Last().P2.Board.Ships.Count +
                                                          " Ships]";
                }

                SaveSystem.SavesList.Add(new List<State>(SaveSystem.GameStates));
                
            }
            SaveSystem.GameStates = new List<State>();
        }

        public static void PlayReplay(Save save)
        {
            var replay = new List<State>();
            for (int i = 0; i < save.States.Count; i++)
            {
                var p1gb = save.States[i].Player1GB.GetDomainBoard();
                var p1map = save.States[i].Player1Map.GetDomainBoard();
                var player1 = save.Player1.GetDomainPlayer(p1gb, p1map);
                var player2 = save.Player2.GetDomainPlayer(save.States[i].Player2GB.GetDomainBoard(),save.States[i].Player2Map.GetDomainBoard());
                State state = save.States[i].GetDomainState(player1,player2,save.Rules.CanTouch);
                state.time = save.States[i].TimeStamp;
                replay.Add(state);
            }

            var rePlayer = new Player("Replay", replay[0].P1.Board, replay[0].P1.Board );
            int index = 0;
            while (true)
            {
                if (index <0)
                {
                    index = 0;
                }

                if (index > replay.Count - 1)
                {
                    break;
                }

                var state = replay[index];
                rePlayer.Name = state.time.ToString();
                Draw(rePlayer,state.P1 + new string(' ', state.P1.Board.Board[0].Count * 4 +3 - state.P1.Name.Length) +"\n" + GetBoardString(state.P1.Board),state.P2 + "\n" + GetBoardString(state.P2.Board));
                Console.WriteLine("---------------------------------------------" +
                                  "\nX - Back, <- Play Backward, Play Forward -> ");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.X:
                        index = replay.Count;
                        break;
                    case ConsoleKey.Enter:
                        continue;
                    case ConsoleKey.RightArrow:
                        index++;
                        continue;
                    case ConsoleKey.LeftArrow:
                        index--;
                        continue;
                }
            }
//            for (int i = 0; i < replay.Count; i++)
//            {
//                var state = replay[i];
//                rePlayer.Name = state.time.ToString();
//                Draw(rePlayer,state.P1 + new string(' ', state.P1.Board.Board[0].Count * 4 +3 - state.P1.Name.Length) +"\n" + GetBoardString(state.P1.Board),state.P2 + "\n" + GetBoardString(state.P2.Board));
//                switch (Console.ReadKey(true).Key)
//                {
//                    case ConsoleKey.Enter:
//                        break;
//                }
//            }
        }

        public static void LoadGame(Save save)
        {
            Console.WriteLine("Loading...");
            if (save.Replay)
            {
                Console.WriteLine("Loading Replay...");
                var ctx = new AppDbContext();
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
            Console.WriteLine("Loading Save...");
            Player player1 = new Domain.Player(save.Player1.Name, 
                save.LastState.Player1GB.GetDomainBoard(),
                save.LastState.Player1Map.GetDomainBoard()){AI = save.Player1.AI};
            
            Player player2 = new Domain.Player(save.Player2.Name, 
                save.LastState.Player2GB.GetDomainBoard(),
                save.LastState.Player2Map.GetDomainBoard()){AI = save.Player2.AI};
            Rules.CanTouch = save.Rules.CanTouch;
            bool p2Turn = save.LastState.P2Turn;
            PlayGame(player1,player2, p2Turn);

        }

        public static void SavePick()
        {
            bool done = false;
            Console.WriteLine("Downloading Saves...");
            var ctx = new AppDbContext();
            var query = ctx.Saves.Include(s => s.Player1).Include(s => s.Player2).ToList();
            var index = query.Count - 1;
            DrawSaves(index,query);
            while (!done)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        index--;
                        break;
                    case ConsoleKey.UpArrow:
                        index++;
                        break;
                    case ConsoleKey.Enter:
                        LoadGame(ctx.Saves.Where(s => s.SaveId.Equals(query[index].SaveId))
                            .Include(s => s.Player1).Include(s => s.Player2)
                            .Include(s => s.LastState)
                                .ThenInclude(s => s.Player1GB).ThenInclude(g =>g.Ships).ThenInclude(s=>s.ShipsLocations)
                            .Include(s => s.LastState)
                                .ThenInclude(s =>s.Player2GB).ThenInclude(g =>g.Ships).ThenInclude(s=>s.ShipsLocations)
                            .Include(s => s.LastState)
                                .ThenInclude(s => s.Player1GB).ThenInclude(g => g.Squares)
                            .Include(s => s.LastState)
                                .ThenInclude(s => s.Player2GB).ThenInclude(g => g.Squares)
                            .Include(s => s.LastState)
                                .ThenInclude(s => s.Player1Map).ThenInclude(g=>g.Squares)
                            .Include(s => s.LastState)
                                .ThenInclude(s =>s.Player2Map).ThenInclude(g=>g.Squares)
                            .Include(s=>s.Rules)
                            .First());
                        break;
                    case ConsoleKey.X:
                        done = true;
                        break;
                    case ConsoleKey.Backspace:
                        Console.WriteLine("Deleting...");
                        var saveid = query[index].SaveId;
                        DAL.Save.DeleteSave(saveid, ctx);
                        query = ctx.Saves.Include(s => s.Player1).Include(s => s.Player2).ToList();
                        break;
                    case ConsoleKey.R:
                        PlayReplay(ctx.Saves.Where(s => s.SaveId.Equals(query[index].SaveId))
                            .Include(s => s.Player1).Include(s => s.Player2).Include(s=>s.Rules)
                            .Include(s=>s.States).ThenInclude(s=>s.Player1GB).ThenInclude(g => g.Squares)
                            .Include(s=>s.States).ThenInclude(s=>s.Player2GB).ThenInclude(g => g.Squares)
                            .Include(s=>s.States).ThenInclude(s=>s.Player1Map).ThenInclude(g => g.Squares)
                            .Include(s=>s.States).ThenInclude(s=>s.Player2Map).ThenInclude(g => g.Squares)
                        .First());
                        break;
                }
                if (index < 0)
                {
                    index = query.Count - 1;
                }

                if (index > query.Count - 1)
                {
                    index = 0;
                }
                DrawSaves(index,query);
            }
        }
        
        private static void DrawSaves(int index, List<DAL.Save> saves)
        {
            Console.Clear();
            Console.WriteLine("Available Saves:\n" +
                              "----------------");
            for (int i = saves.Count -1 ; i > -1; i--)
            {
                var number = saves.Count - (i + 1) + 1;
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" ");
                    Console.Write(number.ToString() + ". ");
                    Console.Write(saves[i].ToString());
                    Console.Write(" ");
                    Console.ResetColor();
                    Console.Write("\n");
                }
                else
                {
                    Console.WriteLine(" "+number.ToString() + ". "+saves[i].ToString());
                }
            }
            Console.WriteLine("--------------------------------------------------\n" +
                              " X - Back, Enter - Load Save, R - Replay");
        }

//        public static Player GetWinner(Player player1, Player player2)
//        {
//            if (player1.Board.Ships.Count == 0)
//            {
//                return player2;
//            }
//
//            if (player2.Board.Ships.Count == 0)
//            {
//                return player2;
//            }
//            return null;
//        }
        
        public static void Draw(Player player,string left,string right, string message = "|", int[] shot = null)
        {
            Console.Clear();

            Console.WriteLine(GetHeader(player));
            
            var leftLines = Regex.Split(left, "\r\n|\r|\n");
            var rightLines = Regex.Split(right, "\r\n|\r|\n");
            for (int i = 0; i < rightLines.Length; i++)
            {
                if (i == leftLines.Length / 2 - 1)
                {
                    Console.WriteLine(leftLines[i] + new string (' ', (GetSeparator().Length - message.Length - 1)/2)+" " + message + new string (' ', (GetSeparator().Length - message.Length - 1)/2) + rightLines[i]);
                }
                else
                {
                    Console.WriteLine(leftLines[i] + GetSeparator() + rightLines[i]);
                }
            }
            for (int i = rightLines.Length; i < leftLines.Length; i++)
            {
                Console.WriteLine(leftLines[i] + GetSeparator());
            }
        }
        
        public static int[] Target(string right, Player player, int[] coords = null,bool targetRight = false)
        {
            if (coords == null)
            {
                coords = new int[]{0,0};
            }
            var board = player.Board;
            var x = (coords[1])* 4 +3 ;
            var y = (coords[0])* 2 + 2;
            var leftlines = Regex.Split(GetBoardString(board), "\r\n|\r|\n");
            var rightlines = new string[leftlines.Length];
            var rightlinessplit = Regex.Split(right, "\r\n|\r|\n");
            for (int i = 0; i < leftlines.Length; i++)
            {
                if (i<rightlinessplit.Length)
                {
                    rightlines[i] = rightlinessplit[i];
                }
                else
                {
                    rightlines[i] = " ";
                }
                
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine(GetHeader(player));
                if (coords[0] > board.Board.Count - 1)
                {
                    coords[0] = 0;
                    y = 2;
                }
                if (coords[1] > board.Board[0].Count - 1)
                {
                    coords[1] = 0;
                    x = 3;
                }
                if (coords[0] < 0)
                {
                    coords[0] = board.Board.Count - 1;
                    y = (board.Board.Count - 1) * 2 + 2;//20
                }
                if (coords[1] < 0)
                {
                    coords[1] = board.Board[0].Count - 1;
                    x = (board.Board[0].Count - 1) * 4 + 3; //39
                }
                DrawTarget(right,player,coords,targetRight);
                
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        y = y + 2;
                        coords[0]++;
                        break;
                    case ConsoleKey.UpArrow:
                        y = y - 2;
                        coords[0]--;
                        break;
                    case ConsoleKey.RightArrow:
                        x = x + 4;
                        coords[1]++;
                        break;
                    case ConsoleKey.LeftArrow:
                        x = x - 4;
                        coords[1]--;
                        break;
                    case ConsoleKey.Enter:
                        return new int[] {(y - 2)/2, (x -2)/4};
                    case ConsoleKey.X:
                        Abort = true;
                        return new int[] {(y - 2)/2, (x -2)/4};
                }
            }
        }
        public static void DrawTarget(string right, Player player, int[] coords,bool targetRight = false)
        {
            var board = player.Board;
            var x = (coords[1])* 4 +3 ;
            var y = (coords[0])* 2 + 2;
            var TargetBG = ConsoleColor.Red;
            var leftlines = Regex.Split(GetBoardString(board), "\r\n|\r|\n");
            var rightlines = new string[leftlines.Length];
            var rightlinessplit = Regex.Split(right, "\r\n|\r|\n");
            for (int i = 0; i < leftlines.Length; i++)
            {
                if (i<rightlinessplit.Length)
                {
                    rightlines[i] = rightlinessplit[i];
                }
                else
                {
                    rightlines[i] = " ";
                }
                
            }
                if (targetRight)
                {

                    for (int i = 0; i < leftlines.Length; i++)
                    {
                        if (coords[0] * 2 + 2 == i)
                        {
                            Console.Write(leftlines[i]);
                            Console.Write(GetSeparator());
                            Console.BackgroundColor = TargetBG;
                            Console.Write(rightlines[i]);
                            Console.ResetColor();
                            Console.Write("\n");

                        }
                        else
                        {
                            Console.Write(leftlines[i]);
                            Console.Write(GetSeparator());
                            Console.Write(rightlines[i].Substring(0, (coords[1])* 4 +3));
                            Console.BackgroundColor = TargetBG;
                            Console.Write(rightlines[i].Substring((coords[1])* 4 +3, 3));
                            Console.ResetColor();
                            Console.Write(rightlines[i].Substring(x + 3, rightlines[i].Length - x - 3));
                            Console.Write("\n");
                            //Console.WriteLine(lines[i]);
                        }
                    }
                }
                else
                {
                    TargetBG = ConsoleColor.Blue;
                    for (int i = 0; i < leftlines.Length; i++)
                    {
                        if (i == y)
                        {
                            Console.BackgroundColor = TargetBG;
                            Console.Write(leftlines[i]);
                            Console.ResetColor();
                            Console.Write(GetSeparator());
                            Console.Write(rightlines[i]);
                            Console.Write("\n");

                        }
                        else
                        {
                            Console.Write(leftlines[i].Substring(0, x));
                            Console.BackgroundColor = TargetBG;
                            Console.Write(leftlines[i].Substring(x, 3));
                            Console.ResetColor();
                            Console.Write(leftlines[i].Substring(x + 3, leftlines[i].Length - x - 3));
                            Console.Write(GetSeparator());
                            Console.Write(rightlines[i]);
                            Console.Write("\n");
                            //Console.WriteLine(lines[i]);
                        }
                    }
                }
            Console.WriteLine(GetFooter(player));
        }

        public static string DrawSwitcher(Player player, Ship selectedShip)
        {   
            StringBuilder message = new StringBuilder();
            message.Append("   1)Switch a Ship\n   2)Rotate\n   3)Remove a Ship\n   4)Random(BETA)\n" +
                           "   --------------\n   Available Ships:\n");
            for (int i = 0; i < player.Ships.Count; i++)
            {
                if (player.Ships[i] == selectedShip)
                {
                    message.Append("-> ");
                }else{message.Append("   ");}
                message.Append(player.Ships[i]+"\n");
            }

            message.Append("   --------------\n");

            return message.ToString();
        }

        public static void FullscreenMessage(string message)
        {
            Console.Clear();
            Console.Write("+"+new string ('-', message.Length + 4));
            Console.Write("+\n|  "+message+"  |\n");
            Console.Write("+"+new string ('-', message.Length + 4)+"+\n");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:
                    break;
            }
            
        }

        public static void Placing(Player player,GameBoard board, int[] coords = null)
        {
            player.Ships = new List<Ship>(Rules.Ships);
            if (coords == null)
            {
                coords = new[] {0, 0};
            }
            bool rotation = false;
            var shipsAmount = player.Ships.Count;
            ConsoleColor shipcolor = ConsoleColor.Green;
            Ship selectedShip = player.Ships[0];
            DrawPlacing(player,selectedShip.Length,rotation,coords,shipcolor, DrawSwitcher(player, selectedShip));
            while (shipsAmount>0)
            {
                if (Abort)
                {
                    break;
                }
                shipsAmount = player.Ships.Count;
                if (shipsAmount == 0)
                {break;
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (rotation)
                        {
                            if (coords[0] == 0)
                            {
                                coords[0] = board.Board.Count - 1 - selectedShip.Length + 1;
                            }
                            else
                            {
                                coords[0]--;
                            }
                        }
                        else
                        {
                            if (coords[0] == 0)
                            {
                                coords[0] = board.Board.Count - 1;
                            }
                            else
                            {
                                coords[0]--;
                            }
                        }
                        
                        break;
                    case ConsoleKey.DownArrow:
                        coords[0]++;
                        break;
                    case ConsoleKey.RightArrow:
                        coords[1]++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (!rotation)
                        {
                            if (coords[1] == 0)
                            {
                                coords[1] = (board.Board[0].Count - 1) - selectedShip.Length + 1;
                            }
                            else
                            {
                                coords[1]--;
                            }
                        }
                        else
                        {
                            if (coords[1] == 0)
                            {
                                coords[1] = (board.Board[0].Count - 1);
                            }
                            else
                            {
                                coords[1]--;
                            }
                        }
                        

                        break;
                    case ConsoleKey.Enter:
                        if (shipcolor == ConsoleColor.Green)
                        {
                            AI.SetPlace(board,coords,selectedShip.Length,rotation);
                            player.Ships.Remove(selectedShip);
                            if (player.Ships.Count != 0)
                            {
                                selectedShip = player.Ships[0];
                            }
                            
                        }
                        break;
                    case ConsoleKey.X:
                        Abort = true;
                        break;
                    case ConsoleKey.D1://switch
                        if (player.Ships.IndexOf(selectedShip) == player.Ships.Count - 1)
                        {
                            selectedShip = player.Ships[0];
                        }
                        else
                        {
                            selectedShip = player.Ships[player.Ships.IndexOf(selectedShip) + 1];
                        }
                        break;
                    case ConsoleKey.D2://rotate
                        if (rotation)
                        {
                            rotation = false;
                        }
                        else
                        {
                            rotation = true;}
                        break;
                    case ConsoleKey.D3:
                        try
                        {
                            var shiptoremove = AI.FindShip(player.Board,Target("Use the arrow keys to select\nEnter - remove", player,coords));
                            foreach (var point in shiptoremove.Locations)
                            {
                                board.Board[point[0]][point[1]] = BoardSquareState.Empty;
                            }
                            player.Ships.Add(shiptoremove);
                            board.Ships.Remove(shiptoremove);
                            selectedShip = shiptoremove;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        break;
                    case ConsoleKey.D4:
                        AI.AIPlacing(player.Ships, board);
                        break;
                }

                if (rotation == false)
                {
                    if (coords[0] > (board.Board.Count - 1))
                    {
                        coords[0] = 0;
                    }
                    if (coords[1] > (board.Board[0].Count - 1) - selectedShip.Length + 1)
                    {
                        coords[1] = 0;
                    }
                }

                if (rotation)
                {
                    if (coords[0] > (board.Board.Count - 1) - selectedShip.Length + 1)
                    {
                        coords[0] = 0;
                    }
                    if (coords[1] > (board.Board[0].Count - 1))
                    {
                        coords[1] = 0;
                    }
                }
                if (AI.CheckPlace(board, coords, selectedShip.Length, rotation))
                {
                    shipcolor = ConsoleColor.Green;
                }
                else
                {
                    shipcolor = ConsoleColor.Red;
                }
                var menu = DrawSwitcher(player, selectedShip);
                
                DrawPlacing(player,selectedShip.Length,rotation,coords,shipcolor, menu);
            }

            if (!Abort)
            {
                var sb = new StringBuilder();
                sb.Append("PRESS ANY KEY TO CONFIRM");
                Draw(player, GetBoardString(board), sb.ToString());
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.X:
                        break;
                }
            }
        }

        public static void DrawPlacing(Player player, int shipLen,
            bool rotation, int[] coords, ConsoleColor shipcolor, string right)
        {
            Console.Clear();
            GameBoard previewBoard = new GameBoard(player.Board.Board.Count,player.Board.Board[0].Count);
            previewBoard = GameBoard.CloneBoard(player.Board);
            AI.SetPlace(previewBoard,coords,shipLen,rotation);
            var lines = Regex.Split(GetBoardString(previewBoard), "\r\n|\r|\n");
            var rightlines = new string[lines.Length];
            var rightlinessplit = Regex.Split(right, "\r\n|\r|\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (i<rightlinessplit.Length)
                {
                    rightlines[i] = rightlinessplit[i];
                }
                else
                {
                    rightlines[i] = " ";
                }
            
            }
            Console.WriteLine(GetHeader(player));
            bool drawship = false;
            if (rotation)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (coords[0] * 2 + 2 == i)
                    {
                        drawship = true;
                    }
                    if (coords[0] * 2 + 2 + shipLen*2 - 1== i)
                    {
                        drawship = false;
                    }

                    if (drawship)
                    {
                        for (int j = 0; j < lines[i].Length; j++)
                        {
                            if (j == coords[1] * 4 + 3)
                            {
                                Console.ForegroundColor = shipcolor;
                            }if (j == coords[1] * 4 + 2 + 4){Console.ResetColor();}
                            Console.Write(lines[i][j]);
                        }
                        Console.Write(GetSeparator());
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write(lines[i]);
                        Console.Write(GetSeparator());
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (coords[0]*2 +2 == i)
                    {
                        for (int j = 0; j < lines[i].Length; j++)
                        {
                            if (j == coords[1]*4 + 3)
                            {
                                Console.ForegroundColor = shipcolor;
                            }

                            if (j ==  coords[1]*4 +2 + shipLen*4 - 1)
                            {
                                Console.ResetColor();
                            }
                            Console.Write(lines[i][j]);
                        }
                        Console.Write(GetSeparator());
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write(lines[i]);
                        Console.Write(GetSeparator());
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                }
            }
            Console.WriteLine(GetFooter(player));
        }

        public static string GetHeader(Player player)
        {
            
            return new string('-',player.Board.Board[0].Count *4 + 3 +GetSeparator().Length/2 - player.ToString().Length/2) + player +
                   new string('-',player.Board.Board[0].Count *4 + 3 +GetSeparator().Length/2 - player.ToString().Length/2);
        }
        public static string GetFooter(Player player)
        {

            return new string('-',player.Board.Board[0].Count *8 + 3+3 +GetSeparator().Length)+"\n   X - Exit and Save";
        }

        public static string GetSeparator()
        {
            return "   |  ";
        }

        public static void Replay()
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
                sb.Append("┼───");
            }

            sb.Append("┼");
            return sb.ToString();
        }

        public static string GetRowWithData(List<BoardSquareState> boardRow)
        {
            var sb = new StringBuilder();
            foreach (var boardSquareState in boardRow)
            {
                sb.Append("│ " + GetBoardSquareStateSymbol(boardSquareState) + " ");
            }

            sb.Append("│");
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