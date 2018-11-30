using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Rules
    {
        public static int Boardrows = 10;
        public static int Boardcolumns = 10;
        public static bool CanTouch = false;
        public static List<Ship> Ships = new List<Ship>()
        {new Ship(5),new Ship(4),new Ship(3),new Ship(2),new Ship(1),};

        public static bool SaveReplays = true;

        public static void SetTouchTrue()
        {
            CanTouch = true;
        }
        public static void SetTouchFalse()
        {
            CanTouch = false;
        }

        public static void SetReplaysTrue()
        {
            SaveReplays = true;
        }

        public static void SetReplaysFalse()
        {
            SaveReplays = false;
        }

        public static void SetRows()
        {
            Console.Clear();
            Console.WriteLine("Input how many rows you want:");
            var input = Console.ReadLine().ToUpper().Trim();
            Boardrows = Int32.Parse(input);
        }
        public static void SetColumns()
        {
            Console.Clear();//j
            Console.WriteLine("Input how many columns you want:");
            var input = Console.ReadLine().ToUpper().Trim();
            Boardcolumns = Int32.Parse(input);
        }

        public static void SetShips()
        {
            var index = 0;
            bool done = false;
            DrawShips(index,Ships);
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
                        Ships.Add(new Ship(1));
                        index = Rules.Ships.Count - 1;
                        break;
                    case ConsoleKey.X:
                        done = true;
                        break;
                    case ConsoleKey.Backspace:
                        Rules.Ships.Remove(Rules.Ships[index]);
                        break;
                    case ConsoleKey.RightArrow:
                        Ships[index].Length++;
                        Ships[index].Health++;
                        break;
                    case ConsoleKey.LeftArrow:
                        Ships[index].Length--;
                        Ships[index].Health--;
                        break;
                }
                if (index < 0)
                {
                    index = Ships.Count - 1;
                }

                if (index > Ships.Count - 1)
                {
                    index = 0;
                }
                DrawShips(index,Ships);
            }
        }

        private static void DrawShips(int index, List<Ship> ships)
        {
            Console.Clear();
            Console.WriteLine("Available Ships:\n" +
                              "----------------");
            for (int i = 0; i < ships.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" ");
                    Console.Write(ships[i]);
                    Console.Write(" ");
                    Console.ResetColor();
                    Console.Write("\n");
                }
                else
                {
                    Console.WriteLine(" "+ships[i]);
                }
            }
            Console.WriteLine("--------------------------------------------------\n" +
                              " X - Back, Enter - Add Ships, Backspace - Delete ship, Arrow Keys - Adjust size");
        }
    }
}