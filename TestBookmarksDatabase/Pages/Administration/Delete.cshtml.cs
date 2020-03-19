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
    public class DeleteModel : PageModel
    {
        private readonly TestBookmarksDatabase.Models.ApplicationDbContext _context;

        public DeleteModel(TestBookmarksDatabase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = await _context.Bookmarks.FindAsync(id);

            if (Bookmark != null)
            {
                _context.Bookmarks.Remove(Bookmark);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
