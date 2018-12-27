using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Shoot : PageModel
    {
        public string Status = "Unknown";
        public void OnGetAsync(string where)
        {
            //string where = Request.Form["where"];
            if (where == null)
            {
                Status = WebUI.ShootAI();
            }
            else
            {
                Status = WebUI.Shoot(where);
            }
        }
    }
}