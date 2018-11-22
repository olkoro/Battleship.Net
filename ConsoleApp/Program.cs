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
using Microsoft.EntityFrameworkCore.Design;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                "The Game is Loading...");

            var ctx = new AppDbContext();
//            foreach (var save in ctx.Saves)
//            {
//                SaveSystem.SavesList.Add(save.States);
//            }
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

//            for (int i = 0; i < SaveSystem.Saves.Count; i++)
//            {
//                ctx.Saves.Add(new AppDbContext.Save()
//                {
//                    SaveId = i,
//                    States = SaveSystem.Saves[i].States
//                });
//            }
//            ctx.SaveChanges();

            


        }


        static void PrintPerson(Player person)
        {
            Console.WriteLine(person);
        }
    }
}