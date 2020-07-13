using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Repositories.Interfaces;

namespace OnlineGallery.DAL.Repositories
{
    internal class LikeRepository : ILikeRepository
    {
        private readonly OnlineGalleryContext _context;

        public LikeRepository(OnlineGalleryContext context)
        {
            _context = context;
        }

        public void AddLike(Like like)
        {
            _context.Likes.Add(like);
        }

        public void RemoveLike(Like like)
        {
            _context.Likes.Remove(like);
        }

        public async Task<bool> Exist(Like like)
        {
            return await _context.Likes
                .AnyAsync(l => l.UserId == like.UserId && l.ImageId == like.ImageId);
        }
    }
}