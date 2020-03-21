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
        Task<Bookmark> Create(Bookmark bookmark);
        Task<Bookmark> Delete(int id);
        Task<Bookmark> Update(int id, Bookmark bookmark);
        Task<IList<BookmarksListViewModel>> List(Guid? ownerId = null, string search = null, string title = null, BookmarkListOrder order = BookmarkListOrder.None, int page = 0, int pagesize = 0);
        Task<Bookmark> Read(int id);
        bool Exists(int id);
    }
}
