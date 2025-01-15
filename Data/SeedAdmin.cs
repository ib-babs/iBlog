namespace iBlog.Data
{
    public class SeedAdmin
    {
        public async static Task EnsureAdminUser(IApplicationBuilder app, string role, string password, string email)
        {
            var appService = app.ApplicationServices.CreateScope().ServiceProvider;
            var context = appService.GetRequiredService<AppIdentityDbContext>();
            var userManager = appService.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = appService.GetRequiredService<RoleManager<IdentityRole>>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            ApplicationUser? user = await userManager.FindByEmailAsync(email);
            IdentityRole adminRole = new(role);
            if (user == null)
            {
                user = new() { UserName = "iblogadmin", Email = email, EmailConfirmed = true};
                await userManager.CreateAsync(user, password);
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await roleManager.CreateAsync(adminRole);
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
