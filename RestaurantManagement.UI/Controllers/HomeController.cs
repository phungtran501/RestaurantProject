using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;
using RestaurantManagement.UI.Models;
using System.Diagnostics;
using System.Linq;

namespace RestaurantManagement.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriesService _categoriesService;
        private readonly IFoodService _foodService;

        public HomeController(ILogger<HomeController> logger, ICategoriesService categoriesService, IFoodService foodService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
            _foodService = foodService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategory();
            ViewBag.Categories = categories;

            var firstCatetogy = categories.FirstOrDefault();

            ViewBag.FirstNameCategory = firstCatetogy.Text;
            var foods = await _foodService.GetFoodByCategory(Convert.ToInt32(firstCatetogy.Value));
            ViewBag.Food = foods;

            var randomFood = await _foodService.GetRandomFood();

            ViewBag.RandomFood = randomFood;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}