using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Services
{
    public interface IBookmarksManager
    {
        Bookmark Create(Bookmark bookmark);
        Bookmark Delete(int id);
        Bookmark Update(int id, Bookmark bookmark);
        ICollection<BookmarksListViewModel> List(Guid? ownerId = null, string search = null, string title = null, BookmarkListOrder order = BookmarkListOrder.None, int page = 0, int pagesize = 0);
        Bookmark Read(int id);
        bool Exists(int id);
    }
}
