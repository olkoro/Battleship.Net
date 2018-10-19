using System;
using System.Collections.Generic;
using MenuSystem;

namespace GameUI
{
    public class MenuUI
    {
        public string Title { get; set; }
        public List<MenuElement> MenuElements { get; set; } = new List<MenuElement>();
        public void Menu()
        {
            var index = 0;
            bool done = false;
            DrawMenu(index,MenuElements);
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
                        RunTheMethod(MenuElements[index].Method);
                        if (MenuElements[index].GoBackAfter == true)
                        {
                            done = true;
                        }
                        break;
                    case ConsoleKey.X:
                        done = true;
                        break;
                }
                if (index < 0)
                {
                    index = MenuElements.Count - 1;
                }

                if (index > MenuElements.Count - 1)
                {
                    index = 0;
                }
                DrawMenu(index,MenuElements);
            }
            
        }

        public void DrawMenu(int index, List<MenuElement> elements)
        {
            Console.Clear();
            Console.WriteLine("--------------"+Title+"--------------");
            for (int i = 0; i < elements.Count; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" ");
                    Console.Write(elements[i]);
                    Console.Write(" ");
                    Console.ResetColor();
                    Console.Write("\n");
                }
                else
                {
                    Console.WriteLine(" "+elements[i]);
                }
            }
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("X - Back, Enter - select, Arrow keys - navigation");
        }
        
        public bool RunTheMethod(Action myMethodName)
        {
            myMethodName();
            return true;
        }
        public void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.ReadKey();
        }
    }
}