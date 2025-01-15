using iBlog.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace iBlog.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserPostsModel(IBlogRepository blogRepository, UserManager<ApplicationUser> userManager) : PageModel
    {
        public PageListView? PageListView { get; set; }
        public string? UserName { get; set; }
        public async Task<IActionResult> OnGet(string? userId, string? category, int blogPage = 1)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null && user.Posts != null)
                {
                    PageListView = new() { Blogs = user.Posts?.OrderBy(b => b.Id).Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Skip((blogPage - 1) * 8).Take(8),
                        PageInfo = new() { CurrentPage = blogPage, ItemPerPage = 8, TotalItem = user.Posts!.Count },
                        Category = category
                    };
                    UserName = user.UserName;
                }

            }
            else
            {
                var blogs = blogRepository.Blogs.ToList();
                PageListView = new() { Blogs = blogs.OrderBy(b => b.Id).Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Skip((blogPage - 1) * 10).Take(10),
                    PageInfo = new() { CurrentPage = blogPage, ItemPerPage = 10, TotalItem = blogs.Count },
                    Category = category
                };
            }

            return Page();
        }
    }
}
