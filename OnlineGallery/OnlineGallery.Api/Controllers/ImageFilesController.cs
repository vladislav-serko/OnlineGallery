using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineGallery.BLL.Services.Interfaces;

namespace OnlineGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFilesController : ControllerBase
    {
        private readonly IImageFilesService _filesService;

        public ImageFilesController(IImageFilesService filesService)
        {
            _filesService = filesService;
        }

        [Route("{id}", Name = "GetImageFile")]
        [HttpGet]
        public async Task<IActionResult> GetImage(string id)
        {
            var file = await _filesService.GetImageFile(id);

            if (file == null)
                return NotFound("File not found");

            return Ok(file);
        }

        [Route("{id}/full", Name = "GetFullImageFile")]
        [HttpGet]
        public async Task<IActionResult> GetFullImage(string id)
        {
            var file = await _filesService.GetFullImageFile(id);

            if (file == null)
                return NotFound("File not found");

            return Ok(file);
        }
    }
}