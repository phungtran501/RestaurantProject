using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Models;
using RestaurantManagement.UI.wwwroot.admin.Utility;
using System.Diagnostics;

namespace RestaurantManagement.UI.Controllers
{
    public class HomeController : BaseController
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