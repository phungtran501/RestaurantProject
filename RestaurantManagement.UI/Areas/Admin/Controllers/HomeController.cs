using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.UI.Areas.System.Controllers
{
    [Area("Admin")]
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            //var categories = call method to get all category

            //

            //pass data from controller to View()
            //c1: View(categories);

            //c2: ViewBag.lsCategory = categories; --> đem View (List<Category>ViewBag.lsCategory)


            //--> loop
            return View();
        }



    }
}

//MVC pattern = kieu mau kien truc