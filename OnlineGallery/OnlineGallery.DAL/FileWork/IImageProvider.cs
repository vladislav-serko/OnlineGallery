using System.IO;
using System.Threading.Tasks;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.DAL.FileWork
{
    public interface IImageProvider
    {
        Task AddImage(Stream file, string path);
        Stream GetImage(string path);
        void DeleteImage(string path, Image image);
        void DeleteDirectory(string path);
        void OptimizeImage(string source, string destination, int maxQuality);
    }
}