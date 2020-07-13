using System;
using System.Collections.Generic;

namespace OnlineGallery.DAL.Models
{
    public class Image
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string Name { get; set; }

        public string NameForCompressed { get; set; }

        public DateTime Published { get; set; } = DateTime.Now;

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}