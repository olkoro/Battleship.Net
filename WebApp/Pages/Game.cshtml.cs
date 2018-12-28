using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Game : PageModel
    {
        public string gbString = "No GB";
        public GameBoard GameBoard = new GameBoard(10,10);
        public GameBoard Map = new GameBoard(10,10);
//        public void OnGet()
//        {
//        }
        public string Status = "Didnt make a move";
        public async Task OnGetAsync(string where)
        {
            GameBoard = WebUI.GetGameBoard()[0];
            Map = WebUI.GetGameBoard()[1];
            if (where == null)
            {
                Status = WebUI.ShootAI();
            }
            else
            {
                Status = WebUI.Shoot(where);
            }
            if (WebUI.P2Turn)
            {
                Status = WebUI.ShootAI();
            }
            if (WebUI.GetWinner() == null)
            {
                gbString = WebUI.GetString();
            }
            else
            {
                gbString = WebUI.GetWinner().ToString() + " Won!";}

        }
    }
}