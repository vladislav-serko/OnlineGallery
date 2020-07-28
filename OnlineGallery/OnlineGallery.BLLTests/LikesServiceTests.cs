using System;
using System.Threading.Tasks;
using Moq;
using OnlineGallery.BLL.Services;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.UnitOfWork;
using Xunit;
using Xunit.Abstractions;

namespace OnlineGallery.BLLTests
{
    public class LikesServiceTests
    {
        private readonly LikesService _likesService;
        private readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();

        public LikesServiceTests()
        {
            _likesService = new LikesService(_uowMock.Object);
        }

        [Fact]
        public async Task AddLike_LikeNotExists_ExecutesSuccessfully()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(false);
            //Act
            await _likesService.AddLike(like.ImageId, like.UserId);
            //Assert
            _uowMock.Verify(x => x.LikeRepository.AddLike(GetLikeValue(like)));
        }

        [Fact]
        public async Task AddLike_LikeExists_ThrowsInvalidOperationException()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(true);
            //Act
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _likesService.AddLike(like.ImageId, like.UserId));
            //Assert
            _uowMock.Verify(x => x.LikeRepository.AddLike(GetLikeValue(like)), Times.Never);
        }

        [Fact]
        public async Task RemoveLike_LikeExists_ExecutesSuccessfully()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(true);
            //Act
            await _likesService.RemoveLike(like.ImageId, like.UserId);
            //Assert
            _uowMock.Verify(x => x.LikeRepository.RemoveLike(GetLikeValue(like)));
        }

        [Fact]
        public async Task RemoveLike_LikeNotExists_ThrowsInvalidOperationException()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(false);
            //Act
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _likesService.RemoveLike(like.ImageId, like.UserId));
            //Assert
            _uowMock.Verify(x => x.LikeRepository.RemoveLike(GetLikeValue(like)), Times.Never);
        }

        [Fact]
        public async Task LikeExist_LikeFound_ReturnsTrue()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(true);
            //Act
            var result = await _likesService.LikeExist(like.ImageId, like.UserId);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LikeExist_LikeNotFound_ReturnsFalse()
        {
            //Arrange
            var like = new Like
            {
                ImageId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            _uowMock.Setup(x => x.LikeRepository.Exist(GetLikeValue(like)))
                .ReturnsAsync(false);
            //Act
            var result = await _likesService.LikeExist(like.ImageId, like.UserId);
            //Assert
            Assert.False(result);
        }

        private static Like GetLikeValue(Like value)
        {
            return It.Is<Like>(l => l.ImageId == value.ImageId && l.UserId == value.UserId);
        }
    }
}
