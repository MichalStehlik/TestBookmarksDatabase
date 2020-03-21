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
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Administration.Bookmarks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;
        private UserManager<IdentityUser<Guid>> _userManager;
        [BindProperty(SupportsGet = true)]
        public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid OwnerFilter { get; set; }
        public List<SelectListItem> UserList { get; set; }
        [BindProperty(SupportsGet = true)]
        public BookmarkListOrder Order { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public IndexModel(IBookmarksManager bookmarksManager, UserManager<IdentityUser<Guid>> userManager)
        {
            _bookmarksManager = bookmarksManager;
            _userManager = userManager;
        }
        public IList<BookmarksListViewModel> Bookmarks { get; set; }

        public void OnGetAsync()
        {
            UserList = _userManager.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).OrderBy(u => u.Text).ToList();
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            Bookmarks = _bookmarksManager.List(OwnerFilter, SearchFilter, null, Order).Result.ToList();
        }
    }
}
