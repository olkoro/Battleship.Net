using System;
using System.Collections.Generic;
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

            var ctx = new AppDbContext();
            List<List<State>> savesList = new List<List<State>>();
            for (int i = 0; i < ctx.Saves.Count(); i++)
            {
                SaveSystem.SavesList.Add(ctx.Saves.ToList()[i].States);
            }
            // Initialize extension method is added in DbInitializer class

//            var menuItemStartSP = ApplicationMenu.ModeMenu.MenuItems.First();
//            menuItemStartSP.CommandToExecute = BattleUI.RunGame;
//            
//            var menuItemStartMP = ApplicationMenu.ModeMenu.MenuItems.Last();
//            menuItemStartMP.CommandToExecute = BattleUI.RunPvPGame;
//            
//            ApplicationMenu.MainMenu.RunMenu();
            
            var menuItemStartMP = TheMenu.StartMenu.MenuElements.First();
            menuItemStartMP.Method = BattleUI.RunPvPGame;
            
            var menuItemStartSP = TheMenu.StartMenu.MenuElements.Last();
            menuItemStartSP.Method = BattleUI.RunGame;
            
            var Saves = TheMenu.MainMenu.MenuElements[1];
            Saves.Method = BattleUI.SavePick;

            TheMenu.MainMenu.Menu();

            for (int i = 0; i < SaveSystem.SavesList.Count; i++)
            {
                ctx.Saves.Add(SaveSystem.Saves[i]);
            }
            


        }


        static void PrintPerson(Player person)
        {
            Console.WriteLine(person);
        }
    }
}