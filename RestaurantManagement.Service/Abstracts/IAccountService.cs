using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IAccountService
    {
        Task<ResponseDatatable> GetListAccount(RequestDatatable requestDatatable);
        Task<ResponseModel> CreateUpdate(AccountViewModel accountViewModel);
        Task DeleteUser(string key);
        Task<AccountViewModel> GetInfomationUser(string userId);
        IEnumerable<SelectListItem> GetUser();
        Task<ResponseModel> RegisterUser(RegisterModel registerModel);
        IEnumerable<SelectListItem> GetUsers();
    }
}