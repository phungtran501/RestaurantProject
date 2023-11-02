using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IUnitOfWork _unitOfWork;

        public MenuController(IMenuService menuService, IUnitOfWork unitOfWork)
        {
            _menuService = menuService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataMenu(RequestDatatable requestDatatable)
        {

            var result = await _menuService.GetAllMenus(requestDatatable);

            return Json(result); // = Ok()
        }

        [HttpGet]
        public async Task<IActionResult> InsertUpdate(int id)
        {

            if (id == 0)
            {
                return View();
            }

            var menu = await _unitOfWork.MenuRepository.GetById(id);

            var menuvm = new MenuViewModel
            {
                Id = menu.Id,
                Name = menu.Name
            };
            return View(menuvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUpdate(MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _menuService.CreateUpdate(menuViewModel);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "menu");
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int key)
        {
            await _menuService.DeleteMenu(key);
            return Json(true);
        }
    }
}
