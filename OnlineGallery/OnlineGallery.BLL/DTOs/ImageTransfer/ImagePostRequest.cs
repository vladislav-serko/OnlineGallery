using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnlineGallery.BLL.DTOs.ImageTransfer
{
    public class ImagePostRequest
    {
        [Required] 
        public string UserId { get; set; }

        [MaxLength(40)]
        public string Description { get; set; }

        [MaxLength(300)]
        public string ShortDescription { get; set; }

        [Required] public IFormFile File { get; set; }
    }
}