using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.wwwroot.admin.Utility;

namespace RestaurantManagement.UI.Controllers
{
    public class CartController : BaseController
    {
        const string SessionKeyCart = "_SessionCart";
        private readonly IFoodService _foodService;

        public CartController(IFoodService foodService) 
        {
            _foodService = foodService;
        }


        
        public async Task<IActionResult> Index()
        {
            var carts = HttpContext.Session.Get<List<CartModel>>(SessionKeyCart);

            if(carts is null)
            {
                ViewBag.Food = null;
                return View();

            }

            var result = await _foodService.GetItemFood(carts);

            ViewBag.Food = result;

            return View();
        }

        [HttpPost]
        public IActionResult SaveItem([FromBody] CartModel cartModel)
        {
            try
            {
                var ls = new List<CartModel>();

                var carts = HttpContext.Session.Get<List<CartModel>>(SessionKeyCart); //A-2, B-4

                if (carts is null)
                {
                    ls.Add(cartModel);
                    HttpContext.Session.Set<List<CartModel>>(SessionKeyCart, ls);

                    return Json(true);
                }

                var oldCart = carts.FirstOrDefault(x => x.Code == cartModel.Code); //3

                if (oldCart is null)
                {
                    carts.Add(cartModel);
                }
                else
                {
                    oldCart.Quantity += cartModel.Quantity;
                }

                HttpContext.Session.Set<List<CartModel>>(SessionKeyCart, carts);


                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] List<CartModel> cartModel)
        {
            try
            {

                HttpContext.Session.Set(SessionKeyCart, cartModel);


                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
