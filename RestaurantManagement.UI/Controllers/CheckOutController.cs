using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.wwwroot.admin.Utility;

namespace RestaurantManagement.UI.Controllers
{
    public class CheckOutController : BaseController
    {
        const string SessionKeyCart = "_SessionCart";
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckOutController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var carts = HttpContext.Session.Get<List<CartModel>>(SessionKeyCart);

            var total = await _cartService.TotalCart(carts);

            ViewData["totalCart"] = total;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CartCheckOut(CartCheckOutDTO cartCheckOutDTO)
        {
            var carts = HttpContext.Session.Get<List<CartModel>>(SessionKeyCart);

            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            cartCheckOutDTO.UserId = user.Id;

            await _cartService.CreateCart(cartCheckOutDTO, carts);

            TempData["AddCartSuccess"] = "Order successful";

            return RedirectToAction("Index", "Home");
        }

    }
}
