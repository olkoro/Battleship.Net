using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public string gbString = "No GB";
        public GameBoard GameBoard = new GameBoard(10,10);
        public void OnGetAsync()
        {
            WebUI.Run();
            gbString = WebUI.GetString();
            GameBoard = WebUI.GetGameBoard()[0];

        }
    }
}