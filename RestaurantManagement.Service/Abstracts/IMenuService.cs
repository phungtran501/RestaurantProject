using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IMenuService
    {
        Task<ResponseModel> CreateUpdate(MenuViewModel menuViewModel);
        Task DeleteMenu(int key);
        Task<ResponseDatatable> GetAllMenus(RequestDatatable requestDatatable);
    }
}