using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Domain.Helper;
using RestaurantManagement.Domain.Model;

namespace RestaurantManagement.UI.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IEmailHelper _emailHelper;

        public ContactController(IEmailHelper emailHelper) 
        {
            _emailHelper = emailHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(EmailRequest emailRequest)
        {
                 await _emailHelper.SendEmail(emailRequest);

            return View(nameof(Index));
        }
    }
}
