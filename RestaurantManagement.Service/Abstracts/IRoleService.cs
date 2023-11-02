using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IRoleService
    {
        Task<ResponseModel> CreateUpdate(RoleViewModel roleViewModel);
        Task<ResponseDatatable> GetListRole(RequestDatatable requestDatatable);
        IEnumerable<SelectListItem> GetRoles();
        Task DeleteRole(string key);
    }
}