using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public GameBoard GameBoard = new GameBoard(10,10);
        public void OnGetAsync(string cmd)
        {
            if (cmd == "random")
            {
                WebUI.Run();
            }
            GameBoard = WebUI.GetGameBoard()[0];

        }
    }
}