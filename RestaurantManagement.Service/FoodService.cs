using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Domain.Helper;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.Service.DTOs.Cart;
using RestaurantManagement.UI.Areas.Admin.Models;
using System.Data;


namespace RestaurantManagement.Service
{
    public class FoodService : IFoodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHandler _imageHandler;
        private readonly IDapperHelper _dapperHelper;

        public FoodService(IUnitOfWork unitOfWork, IImageHandler imageHandler, IDapperHelper dapperHelper)
        {
            _unitOfWork = unitOfWork;
            _imageHandler = imageHandler;
            _dapperHelper = dapperHelper;
        }

        public async Task<ResponseDatatable> GetAllFood(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            var foods = await _unitOfWork.FoodRepository.GetData(f => f.IsActive);

            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("skipItem", requestDatatable.SkipItems, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("pageSize", requestDatatable.PageSize, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("keyWord", requestDatatable.Keyword, DbType.String, ParameterDirection.Input);
            dynamicParameters.Add("totalRecord", 0, DbType.Int32, ParameterDirection.Output);

            var result = await _dapperHelper.ExcuteStoreProcedureReturnList<FoodDTO>("GetAllFood", dynamicParameters);

            var total = dynamicParameters.Get<int>("totalRecord");


            var data = result.Select(x => new
            {
                Id = ActionDatatable(x.Id),
                x.FoodName,
                x.Description,
                x.Price,
                x.Available,
                x.IsActive,
                x.CategoryName


            }).OrderBy(x => x.FoodName).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = foods.Count(),
                RecordsFiltered = foods.Count(),
                Data = data
            };

            return responseDatatable;


        }

        private string ActionDatatable(int id)
        {
            string delete = "<a href=\"#\" title='delete' class='btn-delete'><span class=\"ti-trash\"></span></a>";
            string edit = $"<a href=\"/admin/food/insertupdate?id={id}\" title='edit'><span class=\"ti-pencil\"></span></a>";

            return $"<span data-key=\"{id}\">{edit}&nbsp;{delete}</span>";
        }


        public async Task<ResponseModel> CreateUpdate(FoodModel foodModel)
        {
            var maxId = foodModel.Id;

            if (foodModel.Id is null)
            {

                var food = new Food
                {
                    Available = foodModel.Available,
                    Description = foodModel.Description,
                    Price = foodModel.Price,
                    CategoryId = foodModel.CategoryId,
                    Name = foodModel.Name,
                    IsActive = foodModel.IsActive,
                    Code = CommonHelper.GenerateCode(12)
                };
                
                var result = _unitOfWork.FoodRepository.Insert(food);

                await _unitOfWork.FoodRepository.Commit();

                maxId = food.Id;
            }
            //update
            else
            { 
                var food = await _unitOfWork.FoodRepository.GetById(foodModel.Id);

                food.Name = foodModel.Name;
                food.Description = foodModel.Description;
                food.Price = foodModel.Price;
                food.CategoryId = foodModel.CategoryId;
                food.Available = foodModel.Available;
                food.IsActive = foodModel.IsActive;

                _unitOfWork.FoodRepository.Update(food);
                await _unitOfWork.FoodRepository.Commit();
            }

            await _imageHandler.SaveImage("wwwroot/images/food", new List<IFormFile> { foodModel.Image }, $"{maxId}.png");

            return new ResponseModel
            {
                Status = true,
                Message = foodModel.Id is null ? "Insert successful" : "Update successful",
                StatusType = StatusType.Success,
                Action = foodModel.Id is null ? ActionType.Insert : ActionType.Update
            };

        }

        public async Task DeleteFood(int key)
        {
            var food = await _unitOfWork.FoodRepository.GetById(key);
            food.IsActive = false;
            _unitOfWork.FoodRepository.Update(food);
            await _unitOfWork.FoodRepository.Commit();
        }

        public async Task<IEnumerable<FoodModel>> GetFoodByCategory(int categoryId)
        {
            var foods = await _unitOfWork.FoodRepository.GetData(x => x.CategoryId == categoryId);

            return foods.Select(x => new FoodModel
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Code = x.Code
            });
        }

        public async Task<IEnumerable<FoodModel>> GetRandomFood()
        {
            var foods = await _unitOfWork.FoodRepository.GetData(x => x.IsActive);

            var getFood =  foods.Select(x => new FoodModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Code = x.Code,
            });

            var radomFood = getFood.OrderBy(x => Guid.NewGuid()).Take(4).ToList();

            return radomFood;

        }

        public async Task<FoodModel> GetFoodDetail(string code)
        {
            var foods = await _unitOfWork.FoodRepository.GetData(x => x.Code == code);

            var getFood = foods.Select(x => new FoodModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Available = x.Available,
                Description = x.Description,
                Code = x.Code
            }).FirstOrDefault();

            return getFood;

        }

        public async Task<IEnumerable<SelectListItem>> GetListFood()
        {
            var foods = await _unitOfWork.FoodRepository.GetData(x => x.IsActive);

            var result = foods.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),

            });

            return result;
        }

        public async Task<IEnumerable<FoodByMenuDTO>> GetFoodByMenu()
        {
            List<FoodByMenuDTO> foodByMenuDTOs = new();

            var lsMenu = (await _unitOfWork.MenuRepository.GetData(x => x.IsDisplay && x.IsActive)).Select(x => new
            {
                MenuId = x.Id,
                Name = x.Name
            }).ToList();

            var lsFoods = await _unitOfWork.MenuDetailRepository.Table.Join(_unitOfWork.FoodRepository.Table, x => x.FoodId,
                                                                                          y => y.Id,
                                                                                          (detail, food) => new
                                                                                          {
                                                                                              MenuId = detail.MenuId,
                                                                                              Price = food.Price,
                                                                                              FoodName = food.Name,
                                                                                              Description = food.Description
                                                                                          }).ToListAsync();

            foreach (var item in lsMenu)
            {
                FoodByMenuDTO foodByMenuDTO = new();


                foodByMenuDTO.MenuName = item.Name;

                var foods = lsFoods.Where(x => x.MenuId == item.MenuId).Select(x => new MenuFood
                {
                    FoodName = x.FoodName,
                    Price = x.Price,
                    Description = x.Description
                }).ToList();


                foodByMenuDTO.Foods = foods;

                foodByMenuDTOs.Add(foodByMenuDTO);

            }

            return foodByMenuDTOs;
        }

        public async Task<IEnumerable<CartItemDTO>> GetItemFood(List<CartModel> cartModels)
        {
            List<CartItemDTO> cartItemDTOs = new();

            var foodCodes = cartModels.Select(x => x.Code).ToList();

            var foods = await _unitOfWork.FoodRepository.GetData(x => foodCodes.Contains(x.Code));

            foreach (var item in foods)
            {
                var currentFood = cartModels.FirstOrDefault(x => x.Code == item.Code);

                cartItemDTOs.Add(new CartItemDTO
                {
                    Name = item.Name,
                    Description = item.Description,
                    Code = item.Code,
                    Id = item.Id,
                    Price = item.Price,
                    Quantity = currentFood.Quantity
                });
            }

            return cartItemDTOs;

        }
    }
}
