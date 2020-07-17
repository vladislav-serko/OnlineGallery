using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Users.Authentication
{
    public class RegistrationRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(30)] public string FirstName { get; set; }

        [MaxLength(30)] public string LastName { get; set; }

        [MaxLength(255)] [Required] public string Password { get; set; }
    }
}