using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestBookmarksDatabase.Models
{
    public class Bookmark
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public IdentityUser<Guid> Owner { get; set; }
        [JsonIgnore]
        public ICollection<Bookmark> Bookmarks { get; set; }
    }
}
