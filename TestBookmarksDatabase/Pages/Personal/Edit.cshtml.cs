using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Personal
{
    public class EditModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public EditModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
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
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            if (Bookmark.OwnerId != currentUserId)
            {
                ErrorMessage = "You are not allowed to edit this bookmark.";
                return RedirectToPage("/Index");
            }

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
            var storedBookmark = _bookmarksManager.Read(Bookmark.Id).Result;
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            if (storedBookmark.OwnerId != currentUserId)
            {
                ErrorMessage = "You are not allowed to edit this bookmark.";
                return RedirectToPage("/Index");
            }

            try
            {               
                Bookmark.OwnerId = currentUserId;
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
