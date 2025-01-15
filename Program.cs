global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using iBlog.Data;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Identity;
using iBlog.Models;


var builder = WebApplication.CreateBuilder(args);
var adminPassword = builder.Configuration["AdminPassword"];
var email = builder.Configuration["email"];
var sqlConnection = builder.Configuration["ConnectionStrings:iBlogConn:SqlDb"];
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSqlServer<AppIdentityDbContext>(sqlConnection, opts => opts.EnableRetryOnFailure());
//builder.Services.AddDbContext<AppIdentityDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection") ?? throw new InvalidOperationException("IdentityDbConnection doesn't exist!")));

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
builder.Services.AddTransient<IBlogRepository, EBlogRepository>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddScoped<IAuthorizationHandler, AuthorOrAdminHandler>();


builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.User.RequireUniqueEmail = true;
});

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Blog/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
//await SeedAdmin.EnsureAdminUser(app, Constants.AdminRole, adminPassword!, email!);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");


app.Run();
