using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.ImageTransfer
{
    public class ImageDeleteRequest
    {
        [Required] public string Id { get; set; }

        [Required] public string UserId { get; set; }
    }
}