namespace NamoriTravel.MenuSerivce
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // Claims are already in the JWT token, so no additional processing is needed
            }

            await _next(context);
        }
    }
}
