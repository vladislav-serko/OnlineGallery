using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Users.Authentication
{
    public class AuthenticationRequest
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}