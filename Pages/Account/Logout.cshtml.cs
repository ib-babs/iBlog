using Microsoft.AspNetCore.Mvc.RazorPages;

namespace iBlog.Pages.Account
{
    public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            await signInManager.SignOutAsync();
            return Redirect("/Blog/Index");
        }
    }
}
