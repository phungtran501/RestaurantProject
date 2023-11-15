using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Service.Abstracts;

namespace RestaurantManagement.UI.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IFoodService _foodService;

        public MenuController(IMenuService menuService, IFoodService foodService)
        {
            _menuService = menuService;
            _foodService = foodService;
        }

        public async Task<IActionResult> Index()
        {


            var lsMenu = await _foodService.GetFoodByMenu();

            return View(lsMenu);
        }



    }
}
