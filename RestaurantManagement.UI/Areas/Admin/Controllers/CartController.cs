using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{

    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(ICartService cartService, IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _cartService = cartService;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var foods = await _unitOfWork.FoodRepository.GetData(x => x.IsActive);

            ViewBag.Foods = foods.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),

            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataCart(RequestDatatable requestDatatable)
        {

            var result = await _cartService.GetListCart(requestDatatable);

            return Json(result); // = Ok()
        }

        [HttpGet]
        public async Task<ActionResult> GetDetail(int key)
        {
            var detail = await _cartService.GetDetailByCartId(key);

            return Json(detail);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var cart = await _unitOfWork.CartRepository.GetById(id);

            var resutl = new CartDTO
            {
                Id = cart.Id,
                Note = cart.Note,
                Status = cart.Status,
            };

            var detail = await _cartService.GetDetailByCartId(id);

            ViewBag.Detail = detail;

            return View(resutl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CartDTO cartDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _cartService.UpdateCart(cartDTO);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "cart", new { area = "admin" });
                }
                else
                {
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
