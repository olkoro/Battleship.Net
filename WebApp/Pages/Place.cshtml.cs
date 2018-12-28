using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Place : PageModel
    {
        public string gbString = "No GB";
        public void OnGetAsync()
        {
            WebUI.Run();
            gbString = WebUI.GetString();
        }
    }
}