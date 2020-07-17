using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineGallery.DAL.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Image> Images { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}