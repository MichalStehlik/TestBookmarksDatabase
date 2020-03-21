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

namespace TestBookmarksDatabase.Personal
{
    public class DeleteModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public DeleteModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
        }

        [BindProperty]
        public Bookmark Bookmark { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = await _bookmarksManager.Read((int)id);

            if (Bookmark == null)
            {
                return NotFound();
            }
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            if (Bookmark.OwnerId != currentUserId)
            {
                ErrorMessage = "You are not allowed to delete this bookmark.";
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = _bookmarksManager.Read((int)id).Result;

            if (Bookmark != null)
            {
                var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                if (Bookmark.OwnerId != currentUserId)
                {
                    ErrorMessage = "You are not allowed to delete this bookmark.";
                    return RedirectToPage("/Index");
                }

                try
                {
                    await _bookmarksManager.Delete((int)id);
                    SuccessMessage = "Bookmark has been removed.";
                }
                catch
                {
                    ErrorMessage = "There was error during bookmark removal.";
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
