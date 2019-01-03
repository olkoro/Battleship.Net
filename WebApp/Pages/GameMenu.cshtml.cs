using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages
{
    public class GameMenu : PageModel
    {
        public void OnGet()
        {
            WebUI.Player1.Board = new GameBoard(10,10);
            WebUI.Player2.Board = new GameBoard(10,10);
            WebUI.Player1.Map = new GameBoard(10,10);
            WebUI.Player2.Map = new GameBoard(10,10);

        }
    }
}