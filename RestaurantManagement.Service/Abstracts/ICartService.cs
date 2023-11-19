using RestaurantManagement.Service.DTOs;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface ICartService
    {
        Task CreateCart(CartCheckOutDTO cartCheckOutDTO, List<CartModel> cartModels);
        Task<ResponseModel> UpdateCart(CartDTO cartDTO);
        Task<IEnumerable<CartDetailModel>> GetDetailByCartId(int idCart);
        Task<ResponseDatatable> GetListCart(RequestDatatable requestDatatable);
        Task<int> TotalCart(List<CartModel> cartModels);
    }
}