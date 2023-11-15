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
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(IUnitOfWork unitOfWork, ICategoriesService categoriesService)
        {
            _unitOfWork = unitOfWork;
            _categoriesService = categoriesService;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataCategory(RequestDatatable requestDatatable)
        {

            var result = await _categoriesService.GetAllCaterories(requestDatatable);

            return Json(result); // = Ok()
        }

        [HttpGet]
        public async Task<IActionResult> InsertUpdate(int id)
        {

            if (id == 0)
            {
                return View(new CategoryModel());
            }

            var category = await _unitOfWork.CategoryRepository.GetById(id);

            var catvm = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(catvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUpdate(CategoryModel categoryModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _categoriesService.CreateUpdate(categoryModel);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "categories");
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
            await _categoriesService.DeleteCategory(key);
            return Json(true);
        }
    }
}
