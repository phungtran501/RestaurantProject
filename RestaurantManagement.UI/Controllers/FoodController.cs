using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;

namespace RestaurantManagement.UI.Controllers
{
    public class FoodController : BaseController
    {
        private readonly IFoodService _foodService;
        private readonly ICommentService _commentService;

        public FoodController(IFoodService foodService, ICommentService commentService) 
        {
            _foodService = foodService;
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetFoodByCategoryId(int id)
        {
            var foods = await _foodService.GetFoodByCategory(id);
            return Json(foods);
        }

        public async Task<IActionResult> Detail(string code)
        {
            var food = await _foodService.GetFoodDetail(code);

            var comments = await _commentService.GetCommentByFoodId(food.Id ?? 0);

            var count = comments is null ? 0 : comments.Count();

            ViewData["CountCmt"] = count;

            ViewBag.Comment = comments;
            
            return View(food);
        }
    }
}
