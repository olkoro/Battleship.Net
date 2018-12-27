using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApp
{
    public class WebUI
    {
        public static void Run(IApplicationBuilder app)
        {
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }

        public static void Menu(IApplicationBuilder app)
        {
            app.Run(async (context) => { await context.Response.WriteAsync("1)Start game\n2)Load Game"); });
            
        }
    }
}