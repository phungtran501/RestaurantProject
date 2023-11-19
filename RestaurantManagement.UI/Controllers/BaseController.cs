using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.wwwroot.admin.Utility;

namespace RestaurantManagement.UI.Controllers
{
    public class BaseController : Controller
    {
        const string SessionKeyCart = "_SessionCart";



        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionCart = HttpContext.Session.Get<List<CartModel>>(SessionKeyCart);

            int numberCart = sessionCart is null ? 0 : sessionCart.Count;

            ViewData["SessionCart"] = numberCart;


        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
