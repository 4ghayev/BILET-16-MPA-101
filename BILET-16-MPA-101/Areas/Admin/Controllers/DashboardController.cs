using Microsoft.AspNetCore.Mvc;

namespace BILET_16_MPA_101.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
