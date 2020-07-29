using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.DAL.FileWork
{
    public class ImageProvider : IImageProvider
    {
        public async Task AddImage(Stream file, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await using var stream = File.Create(path);
            await file.CopyToAsync(stream);
        }

        public Stream GetImage(string path)
        {
            return File.Open(path, FileMode.Open);
        }

        public void DeleteImage(string directoryPath, Image image)
        {
            var pathFull = Path.Combine(directoryPath, image.Name);
            var pathCompressed = Path.Combine(directoryPath, image.NameForCompressed);
            if (File.Exists(pathFull))
                File.Delete(pathFull);
            if (File.Exists(pathCompressed))
                File.Delete(pathCompressed);
        }

        public void DeleteDirectory(string path)
        {
            var di = new DirectoryInfo(path);
            if (!di.Exists) return;
            foreach (var file in di.EnumerateFiles()) file.Delete();
            di.Delete();
        }

        public void OptimizeImage(string source, string destination, int maxQuality)
        {
            var file = new FileInfo(source);
            using (var image = new MagickImage(file))
            {
                if (image.Width > image.Height)
                {
                    if (image.Height > maxQuality)
                        image.Resize(0, maxQuality);
                }
                else
                {
                    if (image.Width > maxQuality)
                        image.Resize(maxQuality, 0);
                }

                image.Write(destination);
            }

            var newFile = new FileInfo(destination);
            var optimizer = new ImageOptimizer();
            optimizer.Compress(newFile);
            newFile.Refresh();
        }
    }
}