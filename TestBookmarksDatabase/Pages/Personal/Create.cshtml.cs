using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Personal
{
    public class CreateModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public CreateModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Bookmark = new Bookmark { Url = "https://" };
            return Page();
        }

        [BindProperty]
        public Bookmark Bookmark { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var currentUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (currentUserId != null)
            {
                try
                {
                    Bookmark.OwnerId = Guid.Parse(currentUserId.Value);
                    var bm = await _bookmarksManager.Create(Bookmark);
                    SuccessMessage = "Bookmark has been added.";
                    return RedirectToPage("/Personal/Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ErrorMessage = "Storing of bookmark has failed.";
                    return Page();
                }                
            }
            ModelState.AddModelError("", "There are no user data available.");
            return Page();
        }
    }
}
