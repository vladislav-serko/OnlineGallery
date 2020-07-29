namespace OnlineGallery.BLL.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifetimeMinutes { get; set; }
    }
}