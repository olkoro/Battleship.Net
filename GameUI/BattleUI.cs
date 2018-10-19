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
        //settings
        // -------------
        public static bool Abort;
        static BattleUI()
        {
            
            ApplicationMenu.GameMenu.MenuItems.Insert(0, new MenuItem()
            {
                ShortcutDescription = "numbers",
                LongDescription = "choose the column"
            });
            ApplicationMenu.GameMenu.ClearScreenInMenuStart = false;

            
        }
        public static void RunGame()
        {
            Abort = false;
            var Board1 = new GameBoard(Rules.Boardrows, Rules.Boardcolumns);
            var Board2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Player1 = new Player("Player 1", Board1);
            var Player2 = new Player("AI", Board2);
            Player2.AI = true;
            Console.Clear();
            //player arranging
            Placing(Player1, Board1);
            List<int> shipsList = new List<int>(new int[]{1,2,3,4});
            AI.AIPlacing(new List<Ship>(Rules.Ships), Player2.Board);
            //game starts
            Console.Clear();
            if (!Abort)
            {
                Console.WriteLine("\n\n\n\n\n\n           THE GAME IS STARTED\n           Press any key to continue");
                switch (Console.ReadKey(true).Key)
                {    
                    case ConsoleKey.Enter:
                        break;
                }
            }
            var coords = new int[2]{0,0};
            var status = "|";
            var turn = 0;
            while (true)//cycle
            {
                if(Abort){break;}
                if (turn == 0)
                {
                    Draw(Player1,Board1.GetBoardString(), Map1.GetBoardString());
                    Console.WriteLine("Enter Where to shoot: ");
                    coords = Target(Map1.GetBoardString(), Player1,coords, true);
                    if (Map1.Board[coords[0]][coords[1]] != BoardSquareState.Empty)
                    {
                        continue;
                    }
                    status = GameBoard.Shoot(Board2, coords, Map1);
                    Draw(Player1,Board1.GetBoardString(), Map1.GetBoardString(), status);
                    if (status == "MISS ")
                    {
                        turn = 1;
                    }
                }
                else
                {
                    FullscreenMessage("The AI is making a move.");
                    status = AI.AIShoot(Board1, Map2);
                    Draw(Player1,Board1.GetBoardString(), Map1.GetBoardString(), status);
                    if (status == "MISS ")
                    {
                        turn = 0;
                    }
                }
                //check win
                if (Board1.Ships.Count == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Player2);
                    sb.Append(" won!");
                    FullscreenMessage(sb.ToString());
                    break;
                }

                if (Board2.Ships.Count == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Player1);
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
            }
//            var res = ApplicationMenu.GameMenu.RunMenu();
            
//            return res;
        }
        
        public static void RunPvPGame()
        {
            Abort = false;
            var Board1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Board2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map1 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Map2 = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            var Player1 = new Player("Player 1", Board1);
            var Player2 = new Player("Player 2", Board2);
            Console.Clear();
            //player arranging
            Placing(Player1, Board1);
            Placing(Player2,Board2);
            //game starts
            Console.Clear();
            if (!Abort)
            {
                Console.WriteLine("\n\n\n\n\n\n           THE GAME IS STARTED\n           Press any key to continue");
                switch (Console.ReadKey(true).Key)
                {    
                    case ConsoleKey.Enter:
                        break;
                }
            }
            var coords = new int[2]{0,0};
            var status = "|";
            var turn = 0;
            while (true)//cycle
            {
                if(Abort){break;}
                if (turn == 0)
                {
                    Draw(Player1,Board1.GetBoardString(), Map1.GetBoardString());
                    Console.WriteLine("Enter Where to shoot: ");
                    //coords = GetCoordsFromInput();
                    coords = Target(Map1.GetBoardString(), Player1,coords, true);
                    if (Map1.Board[coords[0]][coords[1]] != BoardSquareState.Empty)
                    {
                        continue;
                    }
                    status = GameBoard.Shoot(Board2, coords, Map1);
                    Draw(Player1,Board1.GetBoardString(), Map1.GetBoardString(), status);
                    if (status == "MISS ")
                    {
                        turn = 1;
                        coords[0] = 0;
                        coords[1] = 0;
                    }
                }
                else
                {
                    Draw(Player2,Board2.GetBoardString(), Map2.GetBoardString());
                    Console.WriteLine("Enter Where to shoot: ");
                    //coords = GetCoordsFromInput();
                    coords = Target(Map2.GetBoardString(),Player2,coords, true);
                    if (Map2.Board[coords[0]][coords[1]] != BoardSquareState.Empty)
                    {
                        continue;
                    }
                    status = GameBoard.Shoot(Board1, coords, Map2);
                    Draw(Player2,Board2.GetBoardString(), Map2.GetBoardString(), status);
                    if (status == "MISS ")
                    {
                        turn = 0;
                        coords[0] = 0;
                        coords[1] = 0;
                    }
                }
                //check win
                if (Board1.Ships.Count == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Player2);
                    sb.Append(" won!");
                    FullscreenMessage(sb.ToString());
                    break;
                }

                if (Board2.Ships.Count == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Player1);
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

                if (status == "MISS ")
                {
                    FullscreenMessage("PASS THE PC TO THE NEXT PLAYER");
                }
            }

            //var res = ApplicationMenu.GameMenu.RunMenu();
            
            //return res;
        }
        
        public static void Draw(Player player,string left,string right, string message = "|", int[] shot = null)
        {
            Console.Clear();
            //Console.WriteLine(MyBoard.GetBoardString());

            Console.WriteLine(GetHeader(player));
            
            var leftLines = Regex.Split(left, "\r\n|\r|\n");
            var rightLines = Regex.Split(right, "\r\n|\r|\n");
            for (int i = 0; i < rightLines.Length; i++)
            {
                if (i == leftLines.Length / 2 - 1)
                {
                    Console.WriteLine(leftLines[i] + new string (' ', 21 / 2 - message.Length /2) + message + new string (' ', 21 / 2 - message.Length /2) + rightLines[i]);
                }
                else
                {
                    Console.WriteLine(leftLines[i] + "          |          " + rightLines[i]);
                }
            }
            for (int i = rightLines.Length; i < leftLines.Length; i++)
            {
                Console.WriteLine(leftLines[i] + "          |          ");
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
                if (coords[0] > Rules.Boardrows - 1)
                {
                    coords[0] = 0;
                    y = 2;
                }
                if (coords[1] > Rules.Boardcolumns - 1)
                {
                    coords[1] = 0;
                    x = 3;
                }
                if (coords[0] < 0)
                {
                    coords[0] = Rules.Boardrows - 1;
                    y = (Rules.Boardrows - 1) * 2 + 2;//20
                }
                if (coords[1] < 0)
                {
                    coords[1] = Rules.Boardcolumns - 1;
                    x = (Rules.Boardcolumns - 1) * 4 + 3; //39
                }
                DrawTarget(right,player,coords,targetRight);
//                if (targetRight)
//                {
//
//                    for (int i = 0; i < leftlines.Length; i++)
//                    {
//                        if (coords[0] * 2 + 2 == i)
//                        {
//                            Console.Write(leftlines[i]);
//                            Console.Write("          |          ");
//                            Console.BackgroundColor = TargetBG;
//                            Console.ForegroundColor =
//                                TargetFG;
//                            Console.Write(rightlines[i]);
//                            Console.ResetColor();
//                            Console.Write("\n");
//
//                        }
//                        else
//                        {
//                            Console.Write(leftlines[i]);
//                            Console.Write("          |          ");
//                            Console.Write(rightlines[i].Substring(0, (coords[1])* 4 +3));
//                            Console.ForegroundColor =
//                                TargetFG;
//                            Console.BackgroundColor = TargetBG;
//                            Console.Write(rightlines[i].Substring((coords[1])* 4 +3, 3));
//                            Console.ResetColor();
//                            Console.Write(rightlines[i].Substring(x + 3, rightlines[i].Length - x - 3));
//                            Console.Write("\n");
//                            //Console.WriteLine(lines[i]);
//                        }
//                    }
//                }
//                else
//                {
//                    TargetBG = ConsoleColor.Blue;
//                    TargetFG = ConsoleColor.Black;
//                    for (int i = 0; i < leftlines.Length; i++)
//                    {
//                        if (i == y)
//                        {
//                            Console.BackgroundColor = TargetBG;
//                            Console.ForegroundColor =
//                                TargetFG;
//                            Console.Write(leftlines[i]);
//                            Console.ResetColor();
//                            Console.Write("          |          ");
//                            Console.Write(rightlines[i]);
//                            Console.Write("\n");
//
//                        }
//                        else
//                        {
//                            Console.Write(leftlines[i].Substring(0, x));
//                            Console.ForegroundColor =
//                                TargetFG;
//                            Console.BackgroundColor = TargetBG;
//                            Console.Write(leftlines[i].Substring(x, 3));
//                            Console.ResetColor();
//                            Console.Write(leftlines[i].Substring(x + 3, leftlines[i].Length - x - 3));
//                            Console.Write("          |          ");
//                            Console.Write(rightlines[i]);
//                            Console.Write("\n");
//                            //Console.WriteLine(lines[i]);
//                        }
//                    }
//                }

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
                if (targetRight)
                {

                    for (int i = 0; i < leftlines.Length; i++)
                    {
                        if (coords[0] * 2 + 2 == i)
                        {
                            Console.Write(leftlines[i]);
                            Console.Write("          |          ");
                            Console.BackgroundColor = TargetBG;
                            Console.ForegroundColor =
                                TargetFG;
                            Console.Write(rightlines[i]);
                            Console.ResetColor();
                            Console.Write("\n");

                        }
                        else
                        {
                            Console.Write(leftlines[i]);
                            Console.Write("          |          ");
                            Console.Write(rightlines[i].Substring(0, (coords[1])* 4 +3));
                            Console.ForegroundColor =
                                TargetFG;
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
                    TargetFG = ConsoleColor.Black;
                    for (int i = 0; i < leftlines.Length; i++)
                    {
                        if (i == y)
                        {
                            Console.BackgroundColor = TargetBG;
                            Console.ForegroundColor =
                                TargetFG;
                            Console.Write(leftlines[i]);
                            Console.ResetColor();
                            Console.Write("          |          ");
                            Console.Write(rightlines[i]);
                            Console.Write("\n");

                        }
                        else
                        {
                            Console.Write(leftlines[i].Substring(0, x));
                            Console.ForegroundColor =
                                TargetFG;
                            Console.BackgroundColor = TargetBG;
                            Console.Write(leftlines[i].Substring(x, 3));
                            Console.ResetColor();
                            Console.Write(leftlines[i].Substring(x + 3, leftlines[i].Length - x - 3));
                            Console.Write("          |          ");
                            Console.Write(rightlines[i]);
                            Console.Write("\n");
                            //Console.WriteLine(lines[i]);
                        }
                    }
                }
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
                                coords[0] = Rules.Boardrows - 1 - selectedShip.Length + 1;
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
                                coords[0] = Rules.Boardrows - 1;
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
                                coords[1] = (Rules.Boardcolumns - 1) - selectedShip.Length + 1;
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
                                coords[1] = (Rules.Boardcolumns - 1);
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
                            AI.SetPlace(board,coords[0],coords[1],selectedShip.Length,rotation);
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
                    if (coords[0] > (Rules.Boardrows - 1))
                    {
                        coords[0] = 0;
                    }
                    if (coords[1] > (Rules.Boardcolumns - 1) - selectedShip.Length + 1)
                    {
                        coords[1] = 0;
                    }
                }

                if (rotation)
                {
                    if (coords[0] > (Rules.Boardrows - 1) - selectedShip.Length + 1)
                    {
                        coords[0] = 0;
                    }
                    if (coords[1] > (Rules.Boardcolumns - 1))
                    {
                        coords[1] = 0;
                    }
                }
                if (AI.CheckPlace(board, coords[0], coords[1], selectedShip.Length, rotation))
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
            GameBoard previewBoard = new GameBoard(Rules.Boardrows,Rules.Boardcolumns);
            previewBoard = GameBoard.CloneBoard(player.Board);
            AI.SetPlace(previewBoard,coords[0],coords[1],shipLen,rotation);
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
                        Console.Write("          |          ");
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write(lines[i]);
                        Console.Write("          |          ");
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
                        Console.Write("          |          ");
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write(lines[i]);
                        Console.Write("          |          ");
                        Console.Write(rightlines[i]);
                        Console.Write("\n");
                    }
                }
            }
            Console.WriteLine(GetFooter(player));
        }

        public static string GetHeader(Player player)
        {
            
            return new string('-',Rules.Boardcolumns *5 + 3 - player.ToString().Length/2) + player +
                   new string('-',Rules.Boardcolumns *5 + 3  - player.ToString().Length/2);
        }
        public static string GetFooter(Player player)
        {

            return new string('-',Rules.Boardcolumns *10 + player.ToString().Length)+"\n   X - Back to Menu, S - Save";
        }
        
    }
}