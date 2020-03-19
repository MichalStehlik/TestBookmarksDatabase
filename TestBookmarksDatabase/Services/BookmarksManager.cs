using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Services
{
    public class BookmarksManager : IBookmarksManager
    {
        private ApplicationDbContext _context;

        public BookmarksManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bookmark Create(Bookmark bookmark)
        {
            var newBookmark = new Bookmark
            {
                Title = bookmark.Title,
                Description = bookmark.Description,
                Url = bookmark.Url,
                OwnerId = bookmark.OwnerId
            };
            _context.Bookmarks.Add(newBookmark);
            _context.SaveChangesAsync();
            return newBookmark;
        }

        public bool Exists(int id)
        {
            return _context.Bookmarks.Any(e => e.Id == id);
        }

        public Bookmark Read(int id)
        {
            var bookmark = _context.Bookmarks.Where(b => b.Id == id).FirstOrDefault();
            return bookmark;
        }

        public ICollection<BookmarksListViewModel> List(Guid? ownerId = null, string search = "", string title = "", BookmarkListOrder order = BookmarkListOrder.None, int page = 0, int pagesize = 0)
        {
            IQueryable<Bookmark> bookmarks = _context.Bookmarks;
            if (!String.IsNullOrEmpty(search))
                bookmarks = bookmarks.Where(b => (b.Title.Contains(search) || b.Description.Contains(search) || b.Url.Contains(search)));
            if (!String.IsNullOrEmpty(title))
                bookmarks = bookmarks.Where(b => (b.Title.Contains(search)));
            switch (order)
            {
                case BookmarkListOrder.Title:
                    bookmarks = bookmarks.OrderBy(b => b.Title);
                    break;
                case BookmarkListOrder.TitleDescending:
                    bookmarks = bookmarks.OrderByDescending(b => b.Title);
                    break;
                default:
                    break;
            }
            if (pagesize != 0)
            {
                bookmarks = bookmarks.Skip(page * pagesize).Take(pagesize);
            }
            return bookmarks.Select(b => new BookmarksListViewModel { Id = b.Id, Title = b.Title, OwnerId = b.OwnerId, Url = b.Url, OwnerUserName = b.Owner.UserName}).ToList();
        }

        Bookmark IBookmarksManager.Delete(int id)
        {
            Bookmark bm = _context.Bookmarks.Where(b => b.Id == id).FirstOrDefault();
            if (bm != null)
            {
                _context.Bookmarks.Remove(bm);
                _context.SaveChanges();
            }
            return bm;
        }

        Bookmark IBookmarksManager.Update(int id, Bookmark input)
        {
            Bookmark bm = _context.Bookmarks.Where(b => b.Id == id).FirstOrDefault();
            if (bm != null)
            {
                _context.Attach(bm).State = EntityState.Modified;
                bm.Title = input.Title;
                bm.Description = input.Description;
                bm.OwnerId = input.OwnerId;
                _context.SaveChanges();
                return bm;
            }
            return null;
        }
    }

    public enum BookmarkListOrder {
        None,
        Title,
        TitleDescending
    }
}
