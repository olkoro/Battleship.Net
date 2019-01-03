using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Pages
{
    public class Saves : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public Saves(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Save> Save { get;set; }

        public async Task OnGetAsync()
        {
            Save = await _context.Saves.Include(s => s.Player1).Include(s => s.Player2).ToListAsync();
        }
    }
}