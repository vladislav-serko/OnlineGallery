using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Users
{
    public class UserUpdateRequest
    {
        [Required] public string Id { get; set; }

        [Required] public string UserName { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }
    }
}