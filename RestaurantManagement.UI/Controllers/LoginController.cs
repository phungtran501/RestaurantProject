using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;
using RestaurantManagement.UI.wwwroot.admin.Utility;

namespace RestaurantManagement.UI.Controllers
{
    public class LoginController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public LoginController(SignInManager<ApplicationUser> signInManager, IAccountService accountService) 
        {
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Validation failed";
                return View(nameof(Index));
            }

            var result = await _accountService.RegisterUser(registerModel);

            if (result.Status && result.StatusType == StatusType.Success)
            {
                ViewBag.Message = result.Message;
                return View(nameof(Index));
            }
            else
            {
                ViewBag.Error = result.Message;
            }

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            { 
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

               
            }

            return View(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
