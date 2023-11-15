using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IFoodService
    {
        Task<ResponseModel> CreateUpdate(FoodModel foodModel);
        Task DeleteFood(int key);
        Task<ResponseDatatable> GetAllFood(RequestDatatable requestDatatable);
        Task<IEnumerable<FoodModel>> GetFoodByCategory(int categoryId);
        Task<IEnumerable<FoodByMenuDTO>> GetFoodByMenu();
        Task<FoodModel> GetFoodDetail(string code);
        Task<IEnumerable<CartItemDTO>> GetItemFood(List<CartModel> cartModels);
        Task<IEnumerable<SelectListItem>> GetListFood();
        Task<IEnumerable<FoodModel>> GetRandomFood();
    }
}