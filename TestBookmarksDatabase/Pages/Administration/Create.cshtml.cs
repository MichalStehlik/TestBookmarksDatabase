using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestBookmarksDatabase.Models;

namespace TestBookmarksDatabase.Administration
{
    public class CreateModel : PageModel
    {
        private readonly TestBookmarksDatabase.Models.ApplicationDbContext _context;

        public CreateModel(TestBookmarksDatabase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Bookmark = new Bookmark { Url = "https://" };
            return Page();
        }

        [BindProperty]
        public Bookmark Bookmark { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Bookmarks.Add(Bookmark);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
