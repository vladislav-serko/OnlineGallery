using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;
using OnlineGallery.DAL.Repositories.Interfaces;

namespace OnlineGallery.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly OnlineGalleryContext _context;

        public ImageRepository(OnlineGalleryContext context)
        {
            _context = context;
        }

        public async Task<Image> Get(string id)
        {
            return await _context.Images.FindAsync(id);
        }

        public Image Add(Image item)
        {
            return _context.Images.Add(item).Entity;
        }

        public void Delete(Image item)
        {
            _context.Images.Remove(item);
        }

        public Task<int> GetLikeCount(string imageId)
        {
            return _context.Likes
                .Where(like => like.ImageId == imageId)
                .CountAsync();
        }

        public Task<PagedData<Image>> GetImagesByUser(string userId, PaginationOptions options)
        {
            var queryable = _context.Images
                .AsQueryable()
                .Where(i => i.User.Id == userId)
                .OrderByDescending(i => i.Published);

            var result = new PagedData<Image>();
            return result.Create(queryable, options);
        }

        public Task<PagedData<Image>> Search(string query, PaginationOptions options)
        {
            var queryable = _context.Images
                .AsQueryable()
                .Where(i => i.ShortDescription.Contains(query) || i.Description.Contains(query))
                .OrderByDescending(i => i.Published);

            var result = new PagedData<Image>();
            return result.Create(queryable, options);
        }
    }
}