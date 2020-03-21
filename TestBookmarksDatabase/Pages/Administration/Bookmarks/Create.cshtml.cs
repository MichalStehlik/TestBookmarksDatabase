using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Bookmarks
{
    public class CreateModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        public List<SelectListItem> UserList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public CreateModel(IBookmarksManager bookmarksManager, UserManager<IdentityUser<Guid>> userManager)
        {
            _bookmarksManager = bookmarksManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserList = _userManager.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).OrderBy(u => u.Text).ToList();
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

            try
            {
                var bm = await _bookmarksManager.Create(Bookmark);
                SuccessMessage = "Bookmark has been added.";
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ErrorMessage = "Storing of bookmark has failed.";
            }                

            return Page();
        }
    }
}
