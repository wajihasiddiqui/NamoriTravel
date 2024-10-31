using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace NamoriTravel.Authorize
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredPermission;
        private readonly string _requiredPage;

        public CustomAuthorizeAttribute(string PageName, string requiredPermission)
        {
            _requiredPermission = requiredPermission;
            _requiredPage = PageName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var pagePermissions = user.Claims
                .Where(c => c.Type == "PagePermission")
                .Select(c => c.Value)
                .ToList();

            if (!HasRequiredPermission(pagePermissions))
            {
                context.Result = new ForbidResult();
            }

        }

        private bool HasRequiredPermission(IList<string> pagePermissions)
        {
            foreach (var permission in pagePermissions)
            {
                var parts = permission.Split(':');
                var pageName = parts[0];
                var pageUrl = parts[1];
                var permissions = parts[2].Split(',');

                if (permissions.Contains(_requiredPermission) && _requiredPage == pageName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
