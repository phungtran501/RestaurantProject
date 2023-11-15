using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Id = ActionDatatable(x.Id),
                Username = x.Username,
                Note = x.Note,
                CreateDate = x.CreateDate.ToString("dd/MM/yyyy"),
                Status = x.Status == 1 ? "Confirm" : "Completed",
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

        private string ActionDatatable(int id)
        {
            string delete = "<a href=\"#\" title='delete' class='btn-delete'><span class=\"ti-trash\"></span></a>";
            string edit = $"<a href=\"/admin/cart/insertupdate?id={id}\" title='edit'><span class=\"ti-pencil\"></span></a>";
            string show = $"<a href=\"#\" title='show' class='btn-show'><span class=\"ti-receipt\"></span></a>";

            return $"<span data-key=\"{id}\">{edit}&nbsp;{delete}&nbsp;{show}</span>";
        }

        public async Task<IEnumerable<CartDetailModel>> GetDetailByCartId(int idCart)
        {

            var lsCart = await _unitOfWork.CartDetailRepository.Table.Where( x => x.CartId == idCart)
                .Join(_unitOfWork.FoodRepository.Table, x => x.FoodId, y => y.Id, (detail, food) => new CartDetailModel
                                                                                        {
                                                                                              FoodName = food.Name,
                                                                                              Quantity = detail.Quantity,
                                                                                              FoodPrice = food.Price,
                                                                                              TotalPrice = detail.Price
                                                                                          }).ToListAsync();


            return lsCart;
        }

        public async Task<ResponseModel> CreateUpdate(CartDTO cartDTO)
        {

            if (cartDTO.Id == 0)
            {
                var cart = new Cart
                {
                    Note = cartDTO.Note,
                    UserId = cartDTO.Username,
                    Status = cartDTO.Status,
                    CreateDate = DateTime.Now
                };

                await _unitOfWork.CartRepository.Insert(cart);
            }
            else 
            {
                var cart = await _unitOfWork.CartRepository.GetById(cartDTO.Id);
                cart.Note = cartDTO.Note;
                cart.Status = cartDTO.Status;
                cart.CreateDate = DateTime.Now;
                
            }
            await _unitOfWork.CartRepository.Commit();

            return new ResponseModel
            {
                Status = true,
                Message = cartDTO.Id == 0 ? "" : "Insert successful",
                StatusType = StatusType.Success,
                Action = cartDTO.Id == 0 ? ActionType.Insert : ActionType.Update
            };
        }

        


    }
}
