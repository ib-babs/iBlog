using iBlog.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace iBlog.Data
{
    public class AuthorOrAdminHandler(UserManager<ApplicationUser> userManager) : AuthorizationHandler<OperationAuthorizationRequirement, BlogModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, BlogModel resource)
        {

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }
            if ((requirement.Name != Constants.EditOperation) && (requirement.Name != Constants.DeleteOperation))
            {
                return Task.CompletedTask;
            }

            if (resource.AuthorId == userManager.GetUserId(context.User) || context.User.IsInRole(Constants.AdminRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
