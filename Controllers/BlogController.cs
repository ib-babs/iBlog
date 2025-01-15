using iBlog.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace iBlog.Controllers
{
    public class BlogController(IBlogRepository blogRepository, SignInManager<ApplicationUser> signInManager, IAuthorizationService authorizationService) : Controller
    {
        [TempData]
        public string ReturnMessage { get; set; } = "";
        private readonly AppIdentityDbContext _context = blogRepository.Context;
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string? category, int blogPage = 1)
        {
            int PageSize = 8;
            int totalItem = 0;
            try
            {
                totalItem = blogRepository.Blogs.Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Count();
            }
            catch (ArgumentNullException) { throw; }
            catch (SqlException) { throw; }
            try
            {

                PageInfo pageInfo = new() { CurrentPage = blogPage, ItemPerPage = PageSize, TotalItem = totalItem };
                return View(new PageListView
                {
                    PageInfo = pageInfo,
                    Blogs = blogRepository.Blogs.OrderBy(b => b.CreatedAt).Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Skip((blogPage - 1) * PageSize).Take(PageSize),
                    Category = category,
                    Categories = blogRepository.Blogs.Select(b => b.Category).Distinct().ToList(),
                    Message = ReturnMessage
                });

            }
            catch (SqlException)
            {

                return RedirectToAction();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBlog(int id)
        {
            try
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (blog != null)
                {
                    return View(blog);
                }
                return NotFound();

            }
            catch (SqlException) { }
            return View();
        }
        [HttpGet]
        public IActionResult PostBlog() => View();

        [HttpPost]
        public async Task<IActionResult> PostBlog(BlogModel blogModel)
        {
            try
            {
                var user = await signInManager.UserManager.GetUserAsync(User);
                blogModel.AuthorId = user!.Id;
                blogModel.Author = user;
                if (!ModelState.IsValid)
                {
                    return View();
                }

                ReturnMessage = "New blog is added.";
                await _context.Blogs.AddAsync(blogModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (SqlException)
            {
                return RedirectToAction();
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditBlog(int id)
        {
            try
            {

                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (blog != null && !(await IsAuthorized(User, blog, Constants.EditOperation)))
                {
                    return Forbid();
                }

                return View(blog);
            }
            catch (SqlException)
            {
                return RedirectToAction();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBlog(int id, BlogModel blogModel)
        {
            try
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (blog != null && !(await IsAuthorized(User, blog, Constants.EditOperation)))
                {
                    return Forbid();
                }
                if (blog != null)
                {
                    blog.Title = blogModel.Title;
                    blog.Content = blogModel.Content;
                    blog.ImageUrlPath = blogModel.ImageUrlPath;
                    blog.Category = blogModel.Category;
                    blog.CreatedAt = blogModel.CreatedAt;
                    _context.Attach(blog).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ReturnMessage = "Blog modified successfully!";
                    return RedirectToAction(nameof(GetBlog), new { id });

                }
                return View();
            }
            catch (SqlException)
            {
                return RedirectToAction();
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBlog(string? cat, int id)
        {
            try
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (blog == null) return NotFound();
                if (!(await IsAuthorized(User, blog, Constants.DeleteOperation)))
                {
                    return Forbid();
                }
                return View(blog);
            }
            catch (SqlException)
            {
                return RedirectToAction();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (blog == null)
                {
                    return NotFound("No blog is found!");
                }
                if (!(await IsAuthorized(User, blog, Constants.DeleteOperation)))
                {
                    return Forbid();
                }
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
                ReturnMessage = "Deleted successfully!";
                if (IsUser())
                    return RedirectToAction(nameof(Index));
                else
                    return Redirect("/Admin/UserPosts");
            }
            catch (SqlException)
            {
                return RedirectToAction();
            }
        }

        private bool IsUser()
        {
            var isAdmin = User.IsInRole(Constants.AdminRole);
            if (isAdmin) return false;
            return true;
        }
        private async Task<bool> IsAuthorized(ClaimsPrincipal user, BlogModel blog, string operationName)
        {
            if (user == null || blog == null)
            {
                return false;
            }
            var isAuthorized = await authorizationService.AuthorizeAsync(user, blog, new OperationAuthorizationRequirement { Name = operationName });
            if (!isAuthorized.Succeeded) return false;
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
