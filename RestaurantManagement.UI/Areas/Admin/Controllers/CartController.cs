using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public CartController(ICartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataCart(RequestDatatable requestDatatable)
        {

            var result = await _cartService.GetListCart(requestDatatable);

            return Json(result); // = Ok()
        }

        [HttpPost]
        public async Task<ActionResult> GetDetail(int key)
        {
            var detail = await _cartService.GetDetailByCartId(key);

            return Json(detail);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUpdate(int id)
        {
            var users = _accountService.GetUsers();

            this.ViewData["user"] = users;

            if (id == 0)
            {
                return View(new CartDTO());
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUpdate(CartDTO cartDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _cartService.CreateUpdate(cartDTO);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "cart", new { area = "admin" });
                }
                else
                {
                    ViewBag.IsEdit = result.Action == ActionType.Insert ? false : true;
                    ViewBag.Error = result.Message;
                }
            }
            else
            {
                ViewBag.Error = "Validate mode invalid";
            }

            return View();
        }
    }
}
