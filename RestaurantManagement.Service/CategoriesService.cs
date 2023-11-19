using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Domain.Helper;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;
using System.Data;


namespace RestaurantManagement.Service
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHandler _imageHandler;

        public CategoriesService(IUnitOfWork unitOfWork, IImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _imageHandler = imageHandler;
        }
        public async Task<ResponseDatatable> GetAllCaterories(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            var categories = await _unitOfWork.CategoryRepository.GetData(category => category.IsActive);

            if (!string.IsNullOrEmpty(requestDatatable.Keyword)) //ad 
            {
                string keyword = requestDatatable.Keyword;
                categories = categories.Where(x => x.Name.Contains(keyword));
            }

            var result = categories.Skip(requestDatatable.SkipItems)
                                    .Take(requestDatatable.PageSize).ToList();

            int totalRecords = categories.Count();

            var data = result.Select(x => new
            {
                x.Id,
                x.Name
            }).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = categories.Count(),
                RecordsFiltered = categories.Count(),
                Data = data
            };

            return responseDatatable;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategory()
        {
            var categories = await _unitOfWork.CategoryRepository.GetData(x => x.IsActive);

            var result = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),

            });

            return result;
        }


        public async Task<ResponseModel> CreateUpdate(CategoryModel categoryModel)
        {
            var category = await _unitOfWork.CategoryRepository.GetSingleByConditionAsync(x => x.Name.ToLower() == categoryModel.Name.ToLower() //true && true
                                                                                   && x.Id != categoryModel.Id);
            if (category is not null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Category name is exist",
                    StatusType = StatusType.Fail,
                    Action = categoryModel.Id is null ? ActionType.Insert : ActionType.Update
                };
            }

            var maxId = categoryModel.Id;

            if (categoryModel.Id is null)
            {
                var cat = new Category
                {
                    Name = categoryModel.Name,
                    IsActive = categoryModel.IsActive
                };

                await _unitOfWork.CategoryRepository.Insert(cat);
                await _unitOfWork.CategoryRepository.Commit();
                maxId = cat.Id;
            }
            else  
            {
                var cat = await _unitOfWork.CategoryRepository.GetById(categoryModel.Id);
                cat.Name = categoryModel.Name;
                _unitOfWork.CategoryRepository.Update(cat);
                await _unitOfWork.CategoryRepository.Commit();
            }

            await _imageHandler.SaveImage("wwwroot/images/category", new List<IFormFile> { categoryModel.CategoryImage }, $"{maxId}.png");

            return new ResponseModel
            {
                Status = true,
                Message = categoryModel.Id is null ? "" : "Insert successful",
                StatusType = StatusType.Success,
                Action = categoryModel.Id is null ? ActionType.Insert : ActionType.Update
            };
        }

        public async Task DeleteCategory(int key)
        {
            var cat = await _unitOfWork.CategoryRepository.GetById(key);
            cat.IsActive = false;
             _unitOfWork.CategoryRepository.Update(cat);
             await _unitOfWork.CategoryRepository.Commit();
        }
    }
}
