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
    public class FoodController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFoodService _foodService;

        public FoodController(ICategoriesService categoriesService, IUnitOfWork unitOfWork, IFoodService foodService)
        {
            _categoriesService = categoriesService;
            _unitOfWork = unitOfWork;
            _foodService = foodService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataFood(RequestDatatable requestDatatable)
        {

            var result = await _foodService.GetAllFood(requestDatatable);

            return Json(result); // = Ok()
        }

        [HttpGet]
        public async Task<IActionResult> InsertUpdate(int id)
        {
            var categories = await _categoriesService.GetCategory();

            this.ViewData["categories"] = categories;

            if (id == 0)
            {
                return View(new FoodModel());
            }


            var food = await _unitOfWork.FoodRepository.GetById(id);

            var foodVm = new FoodModel
            {
                Id = id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                Available = food.Available,
                Description = food.Description,
                Price = food.Price
            };

            return View(foodVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUpdate(FoodModel foodModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _foodService.CreateUpdate(foodModel);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "food", new { area = "admin" });
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int key)
        {
            await _foodService.DeleteFood(key);
            return Json(true);
        }

    }
}
