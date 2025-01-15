using iBlog.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace iBlog.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(UserManager<ApplicationUser> userManager) : PageModel
    {
        [TempData]
        public string Message { get; set; } = String.Empty;
        public void OnGet()
        {}
        
        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            try
            {
                var user = await UserExist(userId);
                if (user != null) {
                    await userManager.DeleteAsync(user);
                    Message = "User is successfully removed!";
                    return RedirectToPage();
                }
            }
            catch (InvalidOperationException) { }
            catch (SqlException) { }

            Message = "Something went wrong!";
            return Page();
        }

        private async Task<ApplicationUser?> UserExist(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null) return user;
            return null;
        }
    }
}
