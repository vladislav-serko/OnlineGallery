using System.IO;
using System.Threading.Tasks;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface IImageFilesService
    {
        Task<(Stream, string)> GetImageFile(string id);
        Task<(Stream, string)> GetFullImageFile(string id);
    }
}