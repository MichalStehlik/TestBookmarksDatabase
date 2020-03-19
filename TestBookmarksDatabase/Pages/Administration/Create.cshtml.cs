using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration
{
    public class CreateModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;

        public CreateModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
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
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _bookmarksManager.Create(Bookmark);

            return RedirectToPage("./Index");
        }
    }
}
