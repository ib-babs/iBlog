using iBlog.Data;

namespace iBlog.Models
{
    public class EBlogRepository(AppIdentityDbContext context) : IBlogRepository
    {
        public AppIdentityDbContext Context { get; } = context;
        public IEnumerable<BlogModel> Blogs { get; } = context.Blogs;
    }
}
