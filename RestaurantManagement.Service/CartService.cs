using Dapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;
using System.Data;


namespace RestaurantManagement.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperHelper _dapperHelper;

        public CartService(IUnitOfWork unitOfWork, IDapperHelper dapperHelper)
        {
            _unitOfWork = unitOfWork;
            _dapperHelper = dapperHelper;
        }

        public async Task CreateCart(CartCheckOutDTO cartCheckOutDTO, List<CartModel> cartModels)
        {
            try
            {
                if (cartCheckOutDTO is not null)
                {
                    var userAddress = new UserAddress
                    {
                        Address = cartCheckOutDTO.Address,
                        UserId = cartCheckOutDTO.UserId,
                        Phone = cartCheckOutDTO.Phone,
                        Fullname = cartCheckOutDTO.Fullname,
                        IsActive = true,
                    };

                    await _unitOfWork.UserAddressRepository.Insert(userAddress);

                    var cart = new Cart
                    {
                        UserId = cartCheckOutDTO.UserId,
                        Note = cartCheckOutDTO.Note,
                        CreateDate = DateTime.Now,
                        Status = (int)StatusCart.Confirm,
                    };

                    await _unitOfWork.CartRepository.Insert(cart);

                    await _unitOfWork.CommitAsync();

                    var foodCodes = cartModels.Select(x => x.Code).ToList();

                    var foods = await _unitOfWork.FoodRepository.GetData(x => foodCodes.Contains(x.Code));

                    foreach (var food in foods)
                    {
                        var currentFood = cartModels.FirstOrDefault(x => x.Code == food.Code);

                        var cartDetail = new CartDetail
                        {
                            CartId = cart.Id,
                            FoodId = food.Id,
                            Price = food.Price,
                            Quantity = currentFood.Quantity,
                        };

                        await _unitOfWork.CartDetailRepository.Insert(cartDetail);
                    }

                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> TotalCart(List<CartModel> cartModels)
        {
            List<CartItemDTO> cartItemDTOs = new();

            var foodCodes = cartModels.Select(x => x.Code).ToList();

            var foods = await _unitOfWork.FoodRepository.GetData(x => foodCodes.Contains(x.Code));

            var total = 0;

            foreach (var item in foods)
            {
                var currentFood = cartModels.FirstOrDefault(x => x.Code == item.Code);

                total = total +  (Convert.ToInt32(item.Price) * currentFood.Quantity);
            }

            return total;

        }

        public async Task<ResponseDatatable> GetListCart(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            var carts = await _unitOfWork.CartRepository.GetData();

            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("skipItem", requestDatatable.SkipItems, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("pageSize", requestDatatable.PageSize, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("keyWord", requestDatatable.Keyword, DbType.String, ParameterDirection.Input);
            dynamicParameters.Add("totalRecord", 0, DbType.Int32, ParameterDirection.Output);

            var result = await _dapperHelper.ExcuteStoreProcedureReturnList<CartDTO>("GetAllCart", dynamicParameters);

            var total = dynamicParameters.Get<int>("totalRecord");

            var data = result.Select(x => new CartDTOGrid
            {
                Id = x.Id,
                Username = x.Username,
                Note = x.Note,
                CreateDate = x.CreateDate.ToString("dd/MM/yyyy"),
                Status = GetStatusCart(x.Status),
            }).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = carts.Count(),
                RecordsFiltered = carts.Count(),
                Data = data
            };

            return responseDatatable;
        }

        private string GetStatusCart(short status)
        {
            switch (status)
            {
                case (short)StatusCart.Confirm:
                    return nameof(StatusCart.Confirm);
                case (short)StatusCart.Completed:
                    return nameof(StatusCart.Completed);
                default:
                    return default;
            }

        }

        //public IEnumerable<SelectListItem> GetStatuss()
        //{
        //    StatusCart statusCart = new StatusCart();

        //    });

        //}


        public async Task<IEnumerable<CartDetailModel>> GetDetailByCartId(int idCart)
        {

            var lsCart = await _unitOfWork.CartDetailRepository.Table
                .Where( x => x.CartId == idCart)
                .Join(_unitOfWork.FoodRepository.Table, 
                x => x.FoodId, 
                y => y.Id, 
                (detail, food) => new CartDetailModel
                                    {
                                        FoodName = food.Name,
                                        Quantity = detail.Quantity,
                                        FoodPrice = food.Price
                                    }).ToListAsync();


            return lsCart;
        }

        public async Task<ResponseModel> UpdateCart(CartDTO cartDTO)
        {
                var cart = await _unitOfWork.CartRepository.GetById(cartDTO.Id);
                cart.Note = cartDTO.Note;
                cart.Status = cartDTO.Status;
                cart.CreateDate = DateTime.Now;
                await _unitOfWork.CartRepository.Insert(cart);

            await _unitOfWork.CartRepository.Commit();

            return new ResponseModel
            {
                Status = true,
                Message = "Update successful",
                StatusType = StatusType.Success,
            };
        }
    }
}
