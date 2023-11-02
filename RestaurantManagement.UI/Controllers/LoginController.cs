using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
