using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;

namespace NamoriTravel.Controllers
{
    [CustomAuthorize("Dashboard", "Visible")]
    public class DashboardController : BaseController
    {
        [CustomAuthorize("Dashboard", "Read")]
        public IActionResult Index()
        {
            ViewBag.TblTitle = "Dashboard";
            return View();
        }
    }
}
