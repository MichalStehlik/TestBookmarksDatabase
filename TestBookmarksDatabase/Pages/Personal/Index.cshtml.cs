using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Personal
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        [BindProperty(SupportsGet = true)]
        public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public BookmarkListOrder Order { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public IndexModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
        }

        public IList<BookmarksListViewModel> Bookmarks { get; set; }

        public void OnGetAsync()
        {
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            Bookmarks = _bookmarksManager.List(currentUserId, SearchFilter, null, Order).Result.ToList();
        }
    }
}
