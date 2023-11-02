using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserAddressController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserAddressService _userAddressService;
        private readonly IUnitOfWork _unitOfWork;

        public UserAddressController(IAccountService accountService, IUserAddressService userAddressService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _userAddressService = userAddressService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> InsertUpdate(int id)
        {
            ViewBag.User = _accountService.GetUser();

            if (id == 0)
            {
                return View(new UserAddressViewModel());
            }

            var userAddress = await _unitOfWork.UserAddressRepository.GetById(id);

            var userAddressVM = new UserAddressViewModel
            {
                Id = id,
                UserId = userAddress.UserId,
                Address = userAddress.Address,
                IsActive = userAddress.IsActive,
                Phone = userAddress.Phone,
                Fullname = userAddress.Fullname
            };

            return View(userAddressVM);
        }

        [HttpPost]
        public async Task<IActionResult> GetDataAccount(RequestDatatable requestDatatable)
        {

            var result = await _userAddressService.GetAllUserAddress(requestDatatable);

            return Json(result); // = Ok()
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUpdate(UserAddressViewModel userAddressViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAddressService.CreateUpdate(userAddressViewModel);
                if (result.Status && result.StatusType == StatusType.Success)
                {
                    return RedirectToAction("Index", "UserAddress");
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
        public async Task<IActionResult> Delete(int key)
        {
            await _userAddressService.DeleteUserAddress(key);
            return Json(true);
        }

    }
}
