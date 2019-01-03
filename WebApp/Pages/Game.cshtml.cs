using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Game : PageModel
    {
        public GameBoard GameBoard;
        public GameBoard Map;
        public Player Player;
        public string Status = null;
        public void OnGet(string where)
        {
            Player = WebUI.Current;
            if (Player.AI)
            {
                Status = WebUI.ShootAI();
            }
            if (where == "go" | where == null)
            {
                //Status = WebUI.ShootAI();
            }
            else
            {
                Status = WebUI.Shoot(where);
            }
            if (WebUI.GetWinner() == null){}
            else
            {
                Status = WebUI.GetWinner().Name;
            }
            GameBoard = Player.Board;
            Map = Player.Map;
        }
    }
}