using Microsoft.AspNetCore.Mvc.RazorPages;
using iBlog.Models;

namespace iBlog.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) : PageModel
    {
        [BindProperty]
        public RegisterUserIdentityData RegisterData { get; set; } = default!;
        public IActionResult OnGet()
        {
            if (signInManager.IsSignedIn(User))
                return Redirect("/");
            return Page();
            ;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(RegisterData.Email))
            {
                var userByMail = await userManager.FindByEmailAsync(RegisterData.Email);
                if (userByMail != null)
                {
                    ModelState.AddModelError("", "Email already exist!");
                }
            }
            if (!string.IsNullOrEmpty(RegisterData.UserName))
            {
                var userName = await userManager.FindByNameAsync(RegisterData.UserName);
                if (userName != null)
                {
                    ModelState.AddModelError("", "Username is taken!");
                }
            }
            if (ModelState.IsValid)
            {

                ApplicationUser user = new() { UserName = RegisterData.UserName, Email = RegisterData.Email, EmailConfirmed = true };
                IdentityRole role = new(Constants.UserRole);
                if (roleManager == null)
                {
                    throw new Exception("Rolemanager is null");
                }
                if (!await roleManager.RoleExistsAsync(Constants.UserRole))
                {
                    await roleManager.CreateAsync(role);
                }

                var result = await userManager.CreateAsync(user, RegisterData.Password);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(user, Constants.UserRole);
                    return RedirectToPage("./Login");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {

                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return Page();
        }



    }
}
