using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Bookmarks
{
    public class EditModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        public List<SelectListItem> UserList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public EditModel(IBookmarksManager bookmarksManager, UserManager<IdentityUser<Guid>> userManager)
        {
            _bookmarksManager = bookmarksManager;
            _userManager = userManager;
        }

        [BindProperty]
        public Bookmark Bookmark { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bookmark = _bookmarksManager.Read((int)id).Result;

            if (Bookmark == null)
            {
                return NotFound();
            }
            UserList = _userManager.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).OrderBy(u => u.Text).ToList();

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {               
                _bookmarksManager.Update(Bookmark.Id, Bookmark);
                SuccessMessage = "Bookmark was stored.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookmarksManager.Exists(Bookmark.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch 
            {
                ErrorMessage = "Bookmark was not stored.";
            }

            return RedirectToPage("./Index");
        }
    }
}
