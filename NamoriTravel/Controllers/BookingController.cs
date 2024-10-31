using Microsoft.AspNetCore.Mvc;
using NamoriTravel.Authorize;

namespace NamoriTravel.Controllers
{

    [CustomAuthorize("Booking", "Visible")]
    public class BookingController : BaseController
    {
        [CustomAuthorize("Booking", "Read")]
        public IActionResult Index()
        {
            ViewBag.TblTitle = "Booking";
            return View();
        }
    }
}
