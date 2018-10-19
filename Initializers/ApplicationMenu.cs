using System.Collections.Generic;
using Domain;
using MenuSystem;

namespace Initializers
{
    public static class ApplicationMenu//static
    {


        public static readonly Menu GameMenu = new Menu()
        {
            Title = "Game",
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    LongDescription = "Save game",
                    ShortcutDescription = "S",
                    Shortcuts = new List<string>(){"S"}
                },
            },
        };

        public static readonly Menu ModeMenu = new Menu()
        {
            Title = "Choose mode",
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    LongDescription = "Single Player",
                    ShortcutDescription = "1",
                    Shortcuts = new List<string>(){"1"},
                    IsDefaultChoice = true
                    
                    
                },
                new MenuItem()
                {
                    LongDescription = "PvP",
                    ShortcutDescription = "2",
                    Shortcuts = new List<string>(){"2"},
                },
                
            },
        };
        public static Menu TouchMenu = new Menu()
        {
            Title = "Can ships touch each other?",
            IsMainMenu = true,
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    LongDescription = "yeas",
                    ShortcutDescription = "1",
                    Shortcuts = new List<string>(){"1"},
                    //CommandToExecute = Rules.SetTouchFalse,
                },
                new MenuItem()
                {
                    LongDescription = "nah",
                    ShortcutDescription = "2",
                    Shortcuts = new List<string>(){"2"},
                    //CommandToExecute = Rules.SetTouchTrue,
                },
            }
        };
        public static Menu RuleMenu = new Menu()
        {
            Title = "Rules menu: Battleships",
            IsMainMenu = true,
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    LongDescription = "Rows",
                    ShortcutDescription = "1",
                    Shortcuts = new List<string>(){"1"},
                    //CommandToExecute = Rules.SetRows,
                },
                new MenuItem()
                {
                    LongDescription = "Columns",
                    ShortcutDescription = "2",
                    Shortcuts = new List<string>(){"2"},
                    //CommandToExecute = Rules.SetColumns,
                },
                new MenuItem()
                {
                    LongDescription = "Can ships touch each other",
                    ShortcutDescription = "3",
                    Shortcuts = new List<string>(){"3"},
                    //CommandToExecute = Rules.SetTouch,
                },
                new MenuItem()
                {
                    LongDescription = "Ships",
                    ShortcutDescription = "4",
                    Shortcuts = new List<string>(){"4"},
                    CommandToExecute = Rules.SetShips,
                },
            }
        };
        public static Menu MainMenu = new Menu()
        {
            Title = "Main menu: Battleships",
            IsMainMenu = true,
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    LongDescription = "New game",
                    ShortcutDescription = "1",
                    Shortcuts = new List<string>(){"1"},
                    IsDefaultChoice = true,
                    CommandToExecute = ModeMenu.RunMenu
                },
                new MenuItem()
                {
                    LongDescription = "Load game",
                    ShortcutDescription = "2",
                    Shortcuts = new List<string>(){"2"},
                    CommandToExecute = null,
                },
                new MenuItem()
                {
                    LongDescription = "Rules & Settings",
                    ShortcutDescription = "3",
                    Shortcuts = new List<string>(){"3"},
                    CommandToExecute = RuleMenu.RunMenu,
                },
            }
        };
    }
}