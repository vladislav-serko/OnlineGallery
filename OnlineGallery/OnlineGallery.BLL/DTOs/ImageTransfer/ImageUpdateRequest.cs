using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.ImageTransfer
{
    public class ImageUpdateRequest
    {
        [Required] public string Id { get; set; }

        [Required] public string UserId { get; set; }
        
        public string ShortDescription { get; set; }

        public string Description { get; set; }
    }
}