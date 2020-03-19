using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Administration
{
    public class IndexModel : PageModel
    {
        private IBookmarksManager _bookmarksManager;

        public IndexModel(IBookmarksManager bookmarksManager)
        {
            _bookmarksManager = bookmarksManager;
        }

        public IList<BookmarksListViewModel> Bookmarks { get;set; }

        public void OnGetAsync()
        {
            Bookmarks = _bookmarksManager.List().ToList();
        }
    }
}
