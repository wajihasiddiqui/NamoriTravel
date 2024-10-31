namespace NamoriTravel.MenuSerivce
{
    public class MenuService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<MenuItem> GetMenuItems()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var menuItems = new List<MenuItem>();

            var pagePermissions = user.Claims
                .Where(c => c.Type == "PagePermission")
                .Select(c => c.Value)
                .ToList();

            foreach (var permission in pagePermissions)
            {
                var parts = permission.Split(':');
                var pageName = parts[0];
                var pageUrl = parts[1];
                var rights = parts[2].Split(',');

                var existingMenuItem = menuItems.FirstOrDefault(m => m.Name == pageName);

                if (existingMenuItem == null)
                {
                    menuItems.Add(new MenuItem
                    {
                        Name = pageName,
                        Url = pageUrl,
                        Permissions = rights.ToList()
                    });
                }
                else
                {
                    existingMenuItem.Permissions.AddRange(rights);
                    existingMenuItem.Permissions = existingMenuItem.Permissions.Distinct().ToList();
                }
            }

            return menuItems;
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<string> Permissions { get; set; }
    }
}
