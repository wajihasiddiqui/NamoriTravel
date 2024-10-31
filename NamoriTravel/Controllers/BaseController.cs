using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ModelsDTO;
using NamoriTravel.ModelsDTO;
using System.Text;

namespace NamoriTravel.Controllers
{
    public class BaseController : Controller
    {
        protected int? UserId { get; private set; }

        public BaseController()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {

                var userIdString = HttpContext.Session.GetString("UserID");
                UserId = Convert.ToInt32(userIdString);
                var allPageList = HttpContext.Session.GetString("All_PageList");
                List<PageDTO> pageDTOs;
                List<PageDTO> newpageDTOs;
                var pageIdsRights = new List<int>();
                if (!string.IsNullOrEmpty(allPageList))
                {
                    pageDTOs = JsonSerializer.Deserialize<List<PageDTO>>(allPageList);
                }
                else
                {
                    pageDTOs = new List<PageDTO>();
                }

                var pagePermissions = User.Claims
                    .Where(c => c.Type == "PagePermission")
                    .Select(c => c.Value)
                    .ToList();

                var permissionsDict = new Dictionary<string, List<string>>();

                foreach (var permission in pagePermissions)
                {
                    var parts = permission.Split(':');
                    if (parts.Length >= 3)
                    {
                        var pageName = parts[0];
                        var rights = parts[2].Split(',').ToList();
                        pageIdsRights.Add(Convert.ToInt32(parts[3]));

                        if (permissionsDict.ContainsKey(pageName))
                        {
                            permissionsDict[pageName].AddRange(rights);
                        }
                        else
                        {
                            permissionsDict[pageName] = new List<string>(rights);
                        }
                    }
                }
                // Filter out items where ParentPageId is in the list of pageIdsToRemove
                pageDTOs = pageDTOs.Where(p => pageIdsRights.Contains(p.Id)).ToList();
                ViewBag.MenuItems = BuildMenu(pageDTOs);
                ViewBag.PagePermissions = permissionsDict;
            }
        }

        public static List<MenuItem> BuildMenu(List<PageDTO> pages)
        {
            var menuItems = new List<MenuItem>();
            var lookup = pages.ToDictionary(p => p.Id, p => p);

            foreach (var page in pages)
            {
                if (page.ParentPageId == null)
                {
                    var menuItem = new MenuItem
                    {
                        Id = page.Id,
                        Name = page.PageName,
                        Url = page.PageURL,
                        Iconsvg = page.IconCss,
                        Children = GetChildren(page.Id, lookup)
                    };
                    menuItems.Add(menuItem);
                }
            }

            return menuItems;
        }
        private static List<MenuItem> GetChildren(int parentId, Dictionary<int, PageDTO> lookup)
        {
            return lookup.Where(p => p.Value.ParentPageId == parentId)
                         .Select(p => new MenuItem
                         {
                             Id = p.Value.Id,
                             Name = p.Value.PageName,
                             Url = p.Value.PageURL,
                             Iconsvg = p.Value.IconCss,
                             Children = GetChildren(p.Value.Id, lookup)
                         })
                         .ToList();
        }

        
    }
}
