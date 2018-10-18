using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuSystem
{
    public class Menu
    {
        public string Title { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public bool ClearScreenInMenuStart { get; set; } = true;
        
        public MenuItem GoBackItem { get; set; } = new MenuItem()
        {
            ShortcutDescription = "X",
            LongDescription = "Back!",
            Shortcuts = new List<string>(){"X", "BACK", "GOBACK", "B"}
        };
        
        public MenuItem QuitToMainItem { get; set; } = new MenuItem()
        {
            ShortcutDescription = "Q",
            LongDescription = "Quit to main menu!",
            Shortcuts = new List<string>(){"Q", "QUIT", "HOME"}
        };

        public bool DisplayQuitToMainMenu { get; set; } = false;
        public bool IsMainMenu { get; set; } = false;

        private void PrintMenu()
        {
            var defaultMenuChoice = MenuItems.FirstOrDefault(m => m.IsDefaultChoice == true);

            if (ClearScreenInMenuStart)
            {
                Console.Clear();
            }
            
            Console.WriteLine("-------- " + Title + "--------");
            foreach (var menuItem in MenuItems)
            {
                if (menuItem.IsDefaultChoice)
                {
                    Console.ForegroundColor =
                        ConsoleColor.Blue;
                    Console.WriteLine(menuItem);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(menuItem);
                }
            }

            Console.WriteLine("--------");
            
            Console.WriteLine(GoBackItem);
            
            if (DisplayQuitToMainMenu)
            {
                Console.WriteLine(QuitToMainItem);
            }

            Console.Write(
                defaultMenuChoice == null ? ">" : "[" + defaultMenuChoice.Shortcuts.First() + "]>"
            );
        }


        private void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.ReadKey();
        }

        public string RunMenu()
        {
            var done = true;
            string input;
            do
            {
                done = false;

                PrintMenu();
                input = Console.ReadLine().ToUpper().Trim();

                // shall we exit from this menu

                if (GoBackItem.Shortcuts.Any(s => s == input))
                {
                    break; // jump out of the loop
                }
                if (DisplayQuitToMainMenu && QuitToMainItem.Shortcuts.Any(s => s.ToUpper() == input))
                {
                    break; // jump out of the loop
                }

                
                // find the correct menu item
                MenuItem item = null;
                item = string.IsNullOrWhiteSpace(input)
                    ? MenuItems.FirstOrDefault(m => m.IsDefaultChoice == true)
                    // dig out item, where this input is in its shortcuts
                    : MenuItems.FirstOrDefault(m => m.Shortcuts.Contains(input));

                if (item == null)
                {
                    Console.WriteLine(input + " was not found in the list of commands!");
                    WaitForUser();
                    continue; // jump back to the start of loop
                }

                // execute the command specified in the menu item
                if (item.CommandToExecute == null)
                {
                    Console.WriteLine(input + " has no command assigned to it!");
                    WaitForUser();
                    continue; // jump back to the start of loop
                }

                // everything should be ok now, lets run it!
                var chosenCommand = item.CommandToExecute();
                input = chosenCommand;

                if (IsMainMenu == false && QuitToMainItem.Shortcuts.Contains(chosenCommand))
                {
                    break;
                }

                if ( !GoBackItem.Shortcuts.Contains(chosenCommand) && 
                      !QuitToMainItem.Shortcuts.Contains(chosenCommand)) 
                    WaitForUser();
            } while (done != true);


            return input;
        }
    }
}