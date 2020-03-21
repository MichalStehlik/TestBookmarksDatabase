using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Bookmarks
{
    public class DetailsModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
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

            Bookmark = _bookmarksManager.Read((int)id).Result;

            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            if (Bookmark.OwnerId != currentUserId)
            {
                ErrorMessage = "You are not allowed to see this bookmark.";
                return Unauthorized();
            }

            if (Bookmark == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
