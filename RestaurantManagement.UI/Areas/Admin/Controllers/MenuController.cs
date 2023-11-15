using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IFoodService _foodService;
        private readonly IMenuDetailService _menuDetailService;

        public MenuController(IMenuService menuService, 
                                IUnitOfWork unitOfWork,
                                IFoodService foodService,
                                IMenuDetailService menuDetailService)
        {
            _menuService = menuService;
            _unitOfWork = unitOfWork;
            _foodService = foodService;
            _menuDetailService = menuDetailService;
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

        [HttpPut]
        public async Task<IActionResult> UpdateDisplay(UpdateDisplayDTO updateDisplayDTO)
        {

            await _menuService.UpdateDisplayMenu(updateDisplayDTO);

            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> GetFoodByMenu(int id)
        {
            var foods = await _menuDetailService.GetFoodByMenu(id);
            return Json(foods);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMenuDetail(int key)
        {
            await _menuDetailService.DeleteMenuDetail(key);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMenuDetail([FromBody]List<MenuDetailModel> menuDetailModels)
        {
            await _menuDetailService.InsertUpdate(menuDetailModels);

            return Json(true);
        }
    }
}
