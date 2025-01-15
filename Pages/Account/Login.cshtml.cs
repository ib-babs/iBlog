using Microsoft.AspNetCore.Mvc.RazorPages;

namespace iBlog.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : PageModel
    {
        [BindProperty]
        public LoginUserIdentityData LoginData { get; set; } = default!;
        [BindProperty]
        public string? ReturnUrl { get; set; }
        public IActionResult OnGet(string returnUrl = "/")
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/Blog/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(LoginData.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, LoginData.Password, isPersistent: true, false);
                    return Redirect(ReturnUrl ?? "/Blog/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is invalid");
                }
            }
            return Page();
        }
    }
}
