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
    public class DetailsModel : PageModel
    {
        private readonly TestBookmarksDatabase.Models.ApplicationDbContext _context;

        public DetailsModel(TestBookmarksDatabase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public Bookmark Bookmark { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = await _context.Bookmarks.FirstOrDefaultAsync(m => m.Id == id);

            if (Bookmark == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
