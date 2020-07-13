using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.DAL
{
    public class OnlineGalleryContext : IdentityDbContext<User>
    {
        public OnlineGalleryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.Images)
                .WithOne(p => p.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasKey(l => new {l.ImageId, l.UserId});

            builder.Entity<Like>()
                .HasOne(l => l.Image)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.ImageId);
                

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Data Source=DESKTOP-LU0TI71\\SQLEXPRESS;Initial Catalog = OnlineGallery;Integrated Security=True;");
        //}
    }
}