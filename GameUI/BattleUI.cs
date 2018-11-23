using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using Domain;
using Initializers;
using MenuSystem;

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
            Player winner = null;
            var coords = new int[2]{0,0};
            var status = "|";
            while (Abort == false)//cycle
            {
                if (!P2turn)
                {
                    coords = Target(Player1.Map.GetBoardString(), Player1,coords, true);
                    if (Player1.Map.Board[coords[0]][coords[1]] != BoardSquareState.Empty || Abort)
                    {
                        continue;
                    }
                    status = GameBoard.Shoot(Player2.Board, coords, Player1.Map);
                    Draw(Player1,Player1.Board.GetBoardString(), Player1.Map.GetBoardString(), status);
                    if (status == "MISS ")
                    {
                        P2turn = true;
                        coords[0] = 0;
                        coords[1] = 0;
                    }
                }
                else
                {
                    if (Player2.AI)
                    {
                        FullscreenMessage("The AI is making a move.");
                        status = AI.AIShoot(Player1.Board, Player2.Map);
                        Draw(Player1,Player1.Board.GetBoardString(), Player1.Map.GetBoardString(), status);
                        if (status == "MISS ")
                        {
                            P2turn = false;
                        }
                    }
                    else
                    {
                        Draw(Player2, Player2.Board.GetBoardString(), Player2.Map.GetBoardString());
                        coords = Target(Player2.Map.GetBoardString(), Player2, coords, true);
                        if (Player2.Map.Board[coords[0]][coords[1]] != BoardSquareState.Empty || Abort)
                        {
                            continue;
                        }

                        status = GameBoard.Shoot(Player1.Board, coords, Player2.Map);
                        Draw(Player2, Player2.Board.GetBoardString(), Player2.Map.GetBoardString(), status);
                        if (status == "MISS ")
                        {
                            P2turn = false;
                            coords[0] = 0;
                            coords[1] = 0;
                        }
                    }
                }
                //check win                
                if (GetWinner(Player1,Player2) != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(GetWinner(Player1,Player2));
                    sb.Append(" won!");
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
                SaveSystem.SavesList.Add(new List<State>(SaveSystem.GameStates));
                
            }
            SaveSystem.GameStates = new List<State>();
        }

        public static void PlayReplay(List<State> replay)
        {
            State state = replay[0];
            for (int i = 0; i < replay.Count; i++)
            {
                state = replay[i];
                Draw(state.P1,state.P1 + new string(' ', state.P1.Board.Board[0].Count * 4 +3 - state.P1.Name.Length) +"\n" + state.P1.Board.GetBoardString(),state.P2 + "\n" + state.P2.Board.GetBoardString());
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        break;
                }
            }
        }

        public static void LoadGame(List<State> save)
        {
            Console.WriteLine("Loading...");
            Player player1 = save.Last().P1;
            Player player2 = save.Last().P2;
            Rules.CanTouch = save.Last().CanTouch;
            bool p2Turn = save.Last().P2Turn;
            PlayGame(player1,player2, p2Turn);

        }

        public static void SavePick()
        {
            var index = 0;
            bool done = false;
            DrawSaves(index,SaveSystem.SavesList);
            while (!done)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        index++;
                        break;
                    case ConsoleKey.UpArrow:
                        index--;
                        break;
                    case ConsoleKey.Enter:
                        LoadGame(SaveSystem.SavesList[index]);
                        break;
                    case ConsoleKey.X:
                        done = true;
                        break;
                    case ConsoleKey.Backspace:
                        SaveSystem.SavesList.Remove(SaveSystem.SavesList[index]);
                        break;
                    case ConsoleKey.R:
                        PlayReplay(SaveSystem.SavesList[index]);
                        break;
                }
                if (index < 0)
                {
                    index = SaveSystem.SavesList.Count - 1;
                }

                if (index > SaveSystem.SavesList.Count - 1)
                {
                    index = 0;
                }
                DrawSaves(index,SaveSystem.SavesList);
            }
        }
        
        private static void DrawSaves(int index, List<List<State>> saves)
        {
            Console.Clear();
            Console.WriteLine("Available Saves:\n" +
                              "----------------");
            for (int i = 0; i < saves.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" ");
                    Console.Write((i+1).ToString() + ". ");
                    Console.Write(saves[i].Last());
                    Console.Write(" ");
                    Console.ResetColor();
                    Console.Write("\n");
                }
                else
                {
                    Console.WriteLine(" "+(i+1).ToString() + ". "+saves[i].Last());
                }
            }
            Console.WriteLine("--------------------------------------------------\n" +
                              " X - Back, Enter - Load Save, Backspace - Delete Save, R - Replay Save");
        }

        public static Player GetWinner(Player player1, Player player2)
        {
            if (player1.Board.Ships.Count == 0)
            {
                return player2;
            }

            if (player2.Board.Ships.Count == 0)
            {
                return player2;
            }
            return null;
        }
        
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
            var TargetBG = ConsoleColor.DarkRed;
            var TargetFG = ConsoleColor.Black;
            var leftlines = Regex.Split(board.GetBoardString(), "\r\n|\r|\n");
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
            var leftlines = Regex.Split(board.GetBoardString(), "\r\n|\r|\n");
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
                Draw(player, board.GetBoardString(), sb.ToString());
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
            var lines = Regex.Split(previewBoard.GetBoardString(), "\r\n|\r|\n");
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
        
    }
}