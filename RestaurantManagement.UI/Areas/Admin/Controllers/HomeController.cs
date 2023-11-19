using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.UI.Areas.Admin.Controllers;

namespace RestaurantManagement.UI.Areas.System.Controllers
{

    public class HomeController : BaseController
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