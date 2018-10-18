using System;

namespace Domain
{
    public class Rules
    {
        public static int Boardrows = 10;
        public static int Boardcolumns = 10;
        public static bool CanTouch = false;

        public Rules()
        {
            
        }

        public Rules(int rows, int columns, bool touch)
        {
            Boardrows = rows;
            Boardcolumns = columns;
            CanTouch = touch;
        }

        public static string SetTouchTrue()
        {
            CanTouch = true;
            return "return";
        }
        public static string SetTouchFalse()
        {
            CanTouch = false;
            return "return";
        }

        public static string SetRows()
        {
            Console.Clear();
            Console.WriteLine("Input how many rows you want:");
            var input = Console.ReadLine().ToUpper().Trim();
            Boardrows = Int32.Parse(input);
            return "return";
        }
        public static string SetColumns()
        {
            Console.Clear();//j
            Console.WriteLine("Input how many columns you want:");
            var input = Console.ReadLine().ToUpper().Trim();
            Boardcolumns = Int32.Parse(input);
            return "return";
        }

        public static string SetTouch()
        {
            Console.Clear();
            var done = false;
            Console.WriteLine("Can they touch?");
            bool selected = true;
            
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("YES");
            Console.ResetColor();
            Console.Write("\n");
            Console.WriteLine("NO");
            
            while (!done)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (selected)
                        {
                            selected = false;
                        }
                        else
                        {
                            selected = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selected)
                        {
                            selected = false;
                        }
                        else
                        {
                            selected = true;
                        }
                        break;
                }
                Console.Clear();
                Console.WriteLine("Can they touch?");
                if (selected)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("YES");
                    Console.ResetColor();
                    Console.Write("\n");
                    Console.WriteLine("NO");
                }
                else
                {
                    Console.WriteLine("YES");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("NO");
                    Console.ResetColor();
                    Console.Write("\n");

                }
            }

            CanTouch = selected;
            return "return";
        }
    }
}