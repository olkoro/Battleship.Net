using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using DAL;
using Domain;
using GameUI;
using Initializers;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                "Hello Battleships!");

            var dbContext = new DbContext();
            // Initialize extension method is added in DbInitializer class
            dbContext.Initialize();

//            var gameUi = new ConnectXUI();
            var menuItemStartSP = ApplicationMenu.ModeMenu.MenuItems.First();
            menuItemStartSP.CommandToExecute = BattleUI.RunGame;
            
            var menuItemStartMP = ApplicationMenu.ModeMenu.MenuItems.Last();
            menuItemStartMP.CommandToExecute = BattleUI.RunPvPGame;
            
            ApplicationMenu.MainMenu.RunMenu();
            
            
        }


        static void PrintPerson(Player person)
        {
            Console.WriteLine(person);
        }
    }
}