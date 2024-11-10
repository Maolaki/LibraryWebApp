using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.AuthService.Application.Filters
{
    public class EnsureAuthenticatedUserFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    message = "User not authenticated."
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
