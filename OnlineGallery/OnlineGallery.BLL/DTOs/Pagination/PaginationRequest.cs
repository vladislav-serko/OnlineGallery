using System.ComponentModel.DataAnnotations;

namespace OnlineGallery.BLL.DTOs.Pagination
{
    public class PaginationRequest
    {
        [Required] public int Page { get; set; }

        [Required] public int ItemCount { get; set; }
    }
}