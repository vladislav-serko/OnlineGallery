using System.IO;
using System.Net;
using System.Net.Http;
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
            var (stream, name) = await _filesService.GetImageFile(id);

            return File(stream, GetContentType(name), name);
        }
        
        [Route("{id}/full", Name = "GetFullImageFile")]
        [HttpGet]
        public async Task<IActionResult> GetFullImage(string id)
        {
            var (stream, name) = await _filesService.GetFullImageFile(id);

            return File(stream, GetContentType(name), name);
        }

        private string GetContentType(string name)
        {
            var ext = Path.GetExtension(name).Replace(".", "");
            return $"image/{ext}";
        }

    }
}