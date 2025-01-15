using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace iBlog.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        [BindProperty]
        public EditProfileForm ProfileForm { get; set; } = default!;
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
        [TempData]
        public string? ReturnMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                var users = await userManager.GetUsersInRoleAsync("User");
                if (users.Count > 0)
                    ApplicationUsers = users;

            }
            catch (SqlException)
            {
                ReturnMessage = "Something went wrong!";
            }
            catch (InvalidOperationException)
            {

                ReturnMessage = "Something went wrong!";
            };
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ReturnMessage = $"{user.UserName} has been successfully deleted!";
                await userManager.DeleteAsync(user);
                return RedirectToPage();
            }
            return Page();
        }
    }
}
