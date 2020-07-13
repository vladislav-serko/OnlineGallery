using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineGallery.Api.Extensions;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.Services.Interfaces;

namespace OnlineGallery.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imageService;
        private readonly ILikesService _likesService;

        public ImagesController(IImagesService imageService, ILikesService likesService)
        {
            _imageService = imageService;
            _likesService = likesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromForm] ImagePostRequest request)
        {
            this.CheckUser(request.UserId);

            var image = await _imageService.AddImage(request);
            await AddInformationToImage(image);

            return Created(Url.Link("GetImage", new {image.Id}), image);
        }

        [Route("{id}", Name = "GetImage")]
        [HttpGet]
        public async Task<IActionResult> GetImage(string id)
        {
            var image = await _imageService.GetImage(id);
            await AddInformationToImage(image);
            return Ok(image);
        }

        [HttpGet]
        public async Task<IActionResult> GetImages([Required] string userId, [Required] int page,
            [Required] int itemCount)
        {
            var request = new PaginationRequest{ItemCount = itemCount, Page = page};
            var images = await _imageService.GetImages(userId, request);

            foreach (var image in images.Data) await AddInformationToImage(image);

            return Ok(images);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchImages([Required] string query, [Required] int page,
            [Required] int itemCount)
        {
            var request = new PaginationRequest{ItemCount = itemCount, Page = page};
            var images = await _imageService.SearchImages(query, request);

            foreach (var image in images.Data) await AddInformationToImage(image);

            return Ok(images);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateImage(ImageUpdateRequest request)
        {
            this.CheckUser(request.UserId);

            await _imageService.Update(request);
            return NoContent();
        }
            
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            var isModerator = User.IsInRole("Moderator");
            await _imageService.DeleteImage(id, this.GetUserId(), isModerator);
            return NoContent();
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeImage([FromForm]string id)
        {
            var userId = this.GetUserId();
            await _likesService.AddLike(id, userId);
            return NoContent();
        }

        [HttpPost("unlike")]
        public async Task<IActionResult> UnlikeImage([FromForm]string id)
        {
            var userId = this.GetUserId();
            await _likesService.RemoveLike(id, userId);
            return NoContent();
        }

        private async Task AddInformationToImage(ImageDto image)
        {
            image.Url = Url.Link("GetImageFile", new {image.Id});
            image.UrlToFull = Url.Link("GetFullImageFile", new {image.Id});
            image.IsLiked = await _likesService.LikeExist(image.Id, this.GetUserId());
        }

    }
}