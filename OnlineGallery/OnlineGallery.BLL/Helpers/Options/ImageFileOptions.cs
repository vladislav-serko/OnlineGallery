using System.Collections.Generic;

namespace OnlineGallery.BLL.Helpers.Options
{
    public class ImageFileOptions
    {
        public string DirectoryPath { get; set; }
        public int MaxSize { get; set; }
        public IEnumerable<string> SupportedExtensions { get; set; }
        public int MaxQuality { get; set; }
    }
}