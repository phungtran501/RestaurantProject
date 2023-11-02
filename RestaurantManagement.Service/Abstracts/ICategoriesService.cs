using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface ICategoriesService
    {
        Task<ResponseModel> CreateUpdate(CategoryModel categoryModel);
        Task DeleteCategory(int key);
        Task<ResponseDatatable> GetAllCaterories(RequestDatatable requestDatatable);
        Task<IEnumerable<SelectListItem>> GetCategory();
    }
}