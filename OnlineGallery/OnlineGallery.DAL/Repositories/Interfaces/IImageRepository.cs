using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;

namespace OnlineGallery.DAL.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> Get(string id);
        Task<PagedData<Image>> GetImagesByUser(string userId, PaginationOptions options);
        Task<PagedData<Image>> Search(string query, PaginationOptions options);
        Image Add(Image item);
        void Delete(Image item);
        Task<int> GetLikeCount(string imageId);
    }
}