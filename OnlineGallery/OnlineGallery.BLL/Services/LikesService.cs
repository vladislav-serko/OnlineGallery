using System;
using System.Threading.Tasks;
using AutoMapper;
using OnlineGallery.BLL.DTOs;
using OnlineGallery.BLL.Services.Interfaces;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.UnitOfWork;

namespace OnlineGallery.BLL.Services
{
    public class LikesService : ILikesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLike(string imageId, string userId)
        {
            var like = new Like {ImageId = imageId, UserId = userId};
            if (await _unitOfWork.LikeRepository.Exist(like))
                throw new InvalidOperationException("Like already exist");

            var user = new {Name = "api works"};

            _unitOfWork.LikeRepository.AddLike(like);
            await _unitOfWork.Commit();
        }

        public async Task RemoveLike(string imageId, string userId)
        {
            var like = new Like {ImageId = imageId, UserId = userId};
            if (!await _unitOfWork.LikeRepository.Exist(like))
                throw new InvalidOperationException("Like does not exist");

            _unitOfWork.LikeRepository.RemoveLike(like);
            await _unitOfWork.Commit();
        }

        public async Task<bool> LikeExist(string imageId, string userId)
        {
            var like = new Like
            {
                ImageId = imageId,
                UserId = userId
            };

            return await _unitOfWork.LikeRepository.Exist(like);
        }
    }
}