using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BaseController : Controller
    {
       public void GetSession()
       {
            //var sessionCart = HttpContext.Session.Get<>

            //ViewData["CartItems"]
       }
    }
}
