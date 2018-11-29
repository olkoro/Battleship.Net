using System.Collections.Generic;
using Domain;
using GameUI;
using MenuSystem;

namespace Initializers
{
    public static class TheMenu
    {
        public static MenuUI StartMenu = new MenuUI()
        {
            Title = "Start menu: Battleships",
            MenuElements = new List<MenuElement>()
            {
                new MenuElement()
                {
                    Title = "PvP",
                },
                new MenuElement()
                {
                    Title = "Single Player",
                },
            }
        };
        public static MenuUI TouchMenu = new MenuUI()
        {
            Title = "Can they touch each other?",
            MenuElements = new List<MenuElement>()
            {
                new MenuElement()
                {
                    Title = "Yeah",
                    Method = Rules.SetTouchTrue,
                    GoBackAfter = true,
                },
                new MenuElement()
                {
                    Title = "Nah",
                    Method = Rules.SetTouchFalse,
                    GoBackAfter = true,
                },
            }
        };
        
        public static MenuUI RuleMenu = new MenuUI()
        {
            Title = "Rules menu: Battleships",
            MenuElements = new List<MenuElement>()
            {
                new MenuElement()
                {
                    Title = "Rows",
                    Method = Rules.SetRows,
                },
                new MenuElement()
                {
                    Title = "Columns",
                    Method = Rules.SetColumns,
                },
                new MenuElement()
                {
                    Title = "Can Touch?",
                    Method = TouchMenu.Menu,
                },
                new MenuElement()
                {
                    Title = "Ships",
                    Method = Rules.SetShips,
                },
            }
        };
        public static MenuUI ReplayMenu = new MenuUI()
        {
            Title = "Replay menu: Battleships",
            MenuElements = new List<MenuElement>()
            {
                new MenuElement()
                {
                    Title = "Watch replays as Player 1",
                },
                new MenuElement()
                {
                    Title = "Watch replays as Player 2",
                },
            }
        };

        public static MenuUI MainMenu = new MenuUI()
        {
            Title = "Main menu: Battleships",
            MenuElements = new List<MenuElement>()
            {
                new MenuElement()
                {
                    Title = "New game",
                    Method = StartMenu.Menu,
                },
                new MenuElement()
                {
                    Title = "Saves & Replays",
                    Method = ReplayMenu.Menu,
                },
                new MenuElement()
                {
                    Title = "Save",
                    Method = DAL.AppDbContext.SaveToDb,
                },
                new MenuElement()
                {
                    Title = "Rules & Settings",
                    Method = RuleMenu.Menu,
                },
            }
        };
    }
}