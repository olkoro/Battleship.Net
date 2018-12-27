using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public void OnGet()
        {
            WebUI.Run();
        }
    }
}