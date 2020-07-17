using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Users
{
    public class UserUpdateRequest
    {
        [Required] public string Id { get; set; }

        [MinLength(3)]
        [MaxLength(20)]
        [Required] public string UserName { get; set; }
        
        [MaxLength(30)]
        [Required] public string FirstName { get; set; }
        
        [MaxLength(30)]
        [Required] public string LastName { get; set; }
    }
}