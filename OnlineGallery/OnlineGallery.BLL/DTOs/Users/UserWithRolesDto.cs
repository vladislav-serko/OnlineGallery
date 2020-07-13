using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineGallery.BLL.DTOs.Users
{
    public class UserWithRolesDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }

    }
}
