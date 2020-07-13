using System.IO;
using System.Threading.Tasks;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface IImageFilesService
    {
        Task<Stream> GetImageFile(string id);
        Task<Stream> GetFullImageFile(string id);
    }
}