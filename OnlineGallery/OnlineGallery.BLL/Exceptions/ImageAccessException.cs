using System;

namespace OnlineGallery.BLL.Exceptions
{
    public class ImageAccessException : InvalidOperationException
    {
        public ImageAccessException(string message) : base(message)
        {
        }
    }
}