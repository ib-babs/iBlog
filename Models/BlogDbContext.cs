namespace iBlog.Models
{
    public class BlogDbContext(DbContextOptions<BlogDbContext> options): DbContext(options)
    {
        public DbSet<BlogModel> Blogs => Set<BlogModel>();
    }
}
