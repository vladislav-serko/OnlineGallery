using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Users.Authentication
{
    public class RegistrationRequest
    {
        [Required] public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required] public string Password { get; set; }
    }
}