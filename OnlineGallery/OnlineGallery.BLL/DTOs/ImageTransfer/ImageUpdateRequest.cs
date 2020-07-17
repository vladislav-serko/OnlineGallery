using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.ImageTransfer
{
    public class ImageUpdateRequest
    {
        [Required] public string Id { get; set; }

        [Required] public string UserId { get; set; }

        [MaxLength(40)] public string ShortDescription { get; set; }

        [MaxLength(300)] public string Description { get; set; }
    }
}