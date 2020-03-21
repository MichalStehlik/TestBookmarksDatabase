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

        public async Task<Bookmark> Create(Bookmark bookmark)
        {
            var newBookmark = new Bookmark
            {
                Title = bookmark.Title,
                Description = bookmark.Description,
                Url = bookmark.Url,
                OwnerId = bookmark.OwnerId
            };
            _context.Bookmarks.Add(newBookmark);
            await _context.SaveChangesAsync();
            return newBookmark;
        }

        public bool Exists(int id)
        {
            return _context.Bookmarks.Any(e => e.Id == id);
        }

        public async Task<Bookmark> Read(int id)
        {
            var bookmark = await _context.Bookmarks.Include(b => b.Owner).Where(b => b.Id == id).FirstOrDefaultAsync();
            return bookmark;
        }

        public async Task<IList<BookmarksListViewModel>> List(Guid? ownerId = null, string search = "", string title = "", BookmarkListOrder order = BookmarkListOrder.None, int page = 0, int pagesize = 0)
        {
            IQueryable<Bookmark> bookmarks = _context.Bookmarks;
            if (!String.IsNullOrEmpty(search))
                bookmarks = bookmarks.Where(b => (b.Title.Contains(search) || b.Description.Contains(search) || b.Url.Contains(search)));
            if (!String.IsNullOrEmpty(title))
                bookmarks = bookmarks.Where(b => (b.Title.Contains(search)));
            if (ownerId != Guid.Empty)
                bookmarks = bookmarks.Where(b => (b.OwnerId == ownerId));
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
            return await bookmarks.Select(b => new BookmarksListViewModel { Id = b.Id, Title = b.Title, OwnerId = b.OwnerId, Url = b.Url, OwnerUserName = b.Owner.UserName}).ToListAsync();
        }

        public async Task<Bookmark> Delete(int id)
        {
            Bookmark bm = await _context.Bookmarks.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (bm != null)
            {
                _context.Bookmarks.Remove(bm);
                _context.SaveChanges();
            }
            return bm;
        }

        public async Task<Bookmark> Update(int id, Bookmark input)
        {
            Bookmark bm = await _context.Bookmarks.Where(b => b.Id == id).FirstOrDefaultAsync();
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
}
