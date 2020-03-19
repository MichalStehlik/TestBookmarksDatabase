using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;

namespace TestBookmarksDatabase.Administration
{
    public class IndexModel : PageModel
    {
        private readonly TestBookmarksDatabase.Models.ApplicationDbContext _context;

        public IndexModel(TestBookmarksDatabase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Bookmark> Bookmark { get;set; }

        public async Task OnGetAsync()
        {
            Bookmark = await _context.Bookmarks.ToListAsync();
        }
    }
}
