using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Game : PageModel
    {
        public string gbString = "No GB";
//        public void OnGet()
//        {
//        }
        public string Status = "Didnt make a move";
        public async Task OnGetAsync()
        {
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