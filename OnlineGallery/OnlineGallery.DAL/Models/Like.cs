namespace OnlineGallery.DAL.Models
{
    public class Like
    {
        public string ImageId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public Image Image { get; set; }
    }
}