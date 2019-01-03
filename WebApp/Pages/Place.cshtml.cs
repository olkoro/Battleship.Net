using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public Player Player;
        public void OnGet(string cmd)
        {
            if (cmd == "random")
            {
                WebUI.PlaceRandomly();
            }

            if (cmd == "switch")
            {
                WebUI.Switch();
            }
            Player = WebUI.Current;
            if (cmd == "pvp")
            {
                WebUI.Player2.AI = false;
                WebUI.Player2.Name = "Player 2";
            }

            if (cmd == "sp")
            {
                WebUI.AIPlace();
                WebUI.Player2.AI = true;
                WebUI.Player2.Name = "AI";
            }
        }
    }
}