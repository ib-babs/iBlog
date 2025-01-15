using iBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace iBlog.Data
{
    public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BlogModel> Blogs => Set<BlogModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogModel>().HasOne(p => p.Author).WithMany(u => u.Posts).HasForeignKey(u => u.AuthorId);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

    }
}
