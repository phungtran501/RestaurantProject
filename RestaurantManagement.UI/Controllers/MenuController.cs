using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
