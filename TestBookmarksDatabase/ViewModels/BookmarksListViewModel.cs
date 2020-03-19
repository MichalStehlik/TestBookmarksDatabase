using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBookmarksDatabase.ViewModels
{
    public class BookmarksListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerUserName { get; set; }
        public string Url { get; set; }
    }
}
