using iBlog.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace iBlog.Pages.Account
{
    public class ProfileModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        public PageListView? PageView { get; set; }
        public ApplicationUser? ProfileUser { get; set; }
        [TempData]
        [BindProperty]
        public string? Message { get; set; }
        [BindProperty]
        public EditProfileForm ProfileForm { get; set; } = default!;

        public async Task<IActionResult> OnGet(string? category, string userId, int blogPage = 1)
        {
            try
            {
                var user = await IsUserExist(userId);
                if (user == null)
                {
                    return Redirect("/");
                }
                var PageSize = 4;
                var userPosts = user.Posts?.OrderBy(b => b.CreatedAt).Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Skip((blogPage - 1) * PageSize).Take(PageSize);
                if (userPosts != null)
                {

                    PageView = new()
                    {
                        Blogs = userPosts,
                        PageInfo = new() { CurrentPage = blogPage, ItemPerPage = PageSize, TotalItem = user.Posts!.Where(b => b.Category == (!string.IsNullOrEmpty(category) ? category : b.Category)).Count() },
                        Categories = user.Posts!.Select(b => b.Category).Distinct().ToList(),
                        Category = category
                    };
                }
                ProfileUser = user;
            }
            catch (SqlException) { }
            return Page();
        }


        public async Task<IActionResult> OnPostUpdatePassword(string userId, [FromForm] PasswordForm Password)
        {
            var user = await IsUserExist(userId);
            if (user == null) return Redirect("/");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {


                var res = await userManager.ChangePasswordAsync(user, Password.OldPassword, Password.NewPassword);
                if (!res.Succeeded)
                {
                    foreach (var err in res.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
                Message = "Password has been modified successfully!";
            }
            catch (Exception) { throw; }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditProfile(string userId)
        {
            var user = await IsUserExist(userId);
            if (user == null)
            {
                return Redirect("/");
            }
            if (ModelState.IsValid)
            {
                user.UserName = ProfileForm.UserName;
                user.Email = ProfileForm.Email;
                user.EmailConfirmed = true;

                try
                {
                    await userManager.UpdateAsync(user);
                    ProfileUser = user;
                    Message = "User Profile has been modified successfully!";
                    return RedirectToPage();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Page();
        }


        private async Task<ApplicationUser?> IsUserExist(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
