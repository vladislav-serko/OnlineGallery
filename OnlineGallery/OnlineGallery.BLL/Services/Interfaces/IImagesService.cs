using System.Threading.Tasks;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.DTOs.Pagination;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImageDto> AddImage(ImagePostRequest request);
        Task<ImageDto> GetImage(string id);
        Task<PaginationResponse<ImageDto>> GetImages(string userId, PaginationRequest request);
        Task<PaginationResponse<ImageDto>> SearchImages(string query, PaginationRequest request);
        Task Update(ImageUpdateRequest request);
        Task DeleteImage(string imageId, string userId, bool deletedByModerator);
    }
}