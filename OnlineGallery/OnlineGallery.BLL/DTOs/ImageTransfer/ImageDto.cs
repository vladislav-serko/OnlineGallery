using System;

namespace OnlineGallery.BLL.DTOs.ImageTransfer
{
    public class ImageDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime Published { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string UrlToFull { get; set; }

        public int LikeCount { get; set; }

        public bool IsLiked { get; set; }
    }
}