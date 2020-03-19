using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration
{
    public class DetailsModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;

        public DetailsModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
        }

        public Bookmark Bookmark { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = _bookmarksManager.Read((int)id);

            if (Bookmark == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
