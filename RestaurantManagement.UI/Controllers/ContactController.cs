using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
