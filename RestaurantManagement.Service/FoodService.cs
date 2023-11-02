using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Domain.Helper;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            }).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = foods.Count(),
                RecordsFiltered = result.Count(),
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
                Description = x.Description
            }).FirstOrDefault();

            return getFood;

        }
    }
}
