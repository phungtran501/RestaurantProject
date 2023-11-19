using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;
using RestaurantManagement.UI.Helper;
using System.Net;
using System.Text;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;



        public AccountController(IAccountService accountService,
                                    IRoleService roleService,
                                    UserManager<ApplicationUser> userManager) 
        {
            _accountService = accountService;
            _roleService = roleService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataAccount(RequestDatatable requestDatatable)
        {

            var result = await  _accountService.GetListAccount(requestDatatable);
            
            return Json(result); // = Ok()
        }

        [HttpGet]
        public async Task<IActionResult> CreateUpdate(string id)
        {
            ViewBag.Roles = _roleService.GetRoles();

            if (string.IsNullOrEmpty(id))
            {
                return View(new AccountViewModel());
            }

            var user = await _userManager.FindByIdAsync(id);

            var accountVM = new AccountViewModel
            {
                Address = user.Address,
                Email = user.Email,
                Fullname = user.Fullname,
                Password = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                IsActive = user.IsActive,
                IsSystem = user.IsSystem,
                Id = id,
                RoleId = ""
            };

            return View(accountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpdate(AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                //insert
                var result = await _accountService.CreateUpdate(accountViewModel);

                if (result.Status  && result.StatusType == StatusType.Success)
                {

                    return RedirectToAction("Index", "account");
                }
                else
                {
                    //ternary condition ? :
                    ViewBag.IsEdit = result.Action == ActionType.Insert ? false : true;
                    ViewBag.Roles = _roleService.GetRoles();
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
        public async Task<IActionResult> Delete(string key)
        {
            await _accountService.DeleteUser(key);
            return Json(true);
        }

        [HttpGet("get-infomation-user/{id}")]
        public async Task<ActionResult> GetInfomationUser(string id)
        {
            var user = await _accountService.GetInfomationUser(id);

            return Json(user);
        }
    }
}

//MVC Pattern
