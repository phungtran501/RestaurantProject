using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(IRoleService roleService, RoleManager<IdentityRole> roleManager) 
        {
            _roleService = roleService;
            _roleManager = roleManager;
 
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataRole(RequestDatatable requestDatatable)
        {

            var result = await _roleService.GetListRole(requestDatatable);

            return Json(result);

        }

        [HttpGet]
        public async Task<IActionResult> CreateUpdate(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.IsEdit = false;
                return View();
            }

            ViewBag.IsEdit = true;

            var role = await _roleManager.FindByIdAsync(id);

            var roleVM = new RoleViewModel
            {
                Id = id,
                Name = role.Name
                
            };
            return View(roleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpdate(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //insert
                var result = await _roleService.CreateUpdate(roleViewModel);

                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "role");
                }
                else
                {
                    ViewBag.IsEdit = result.Action == ActionType.Insert ? false : true;
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
            await _roleService.DeleteRole(key);

            return Json(true);
        }
    }
}
