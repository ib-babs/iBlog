using iBlog.Data;
namespace iBlog.Models
{
    public interface IBlogRepository
    {
        AppIdentityDbContext Context { get;  }
        IEnumerable<BlogModel> Blogs { get;  }
    }
}
