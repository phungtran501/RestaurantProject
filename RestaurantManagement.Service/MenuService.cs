﻿using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;


namespace RestaurantManagement.Service
{
    public class MenuService : IMenuService
    {
        IUnitOfWork _unitOfWork;
        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<ResponseDatatable> GetAllMenus(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            var menus = await _unitOfWork.MenuRepository.GetData(menu => menu.IsActive);

            if (!string.IsNullOrEmpty(requestDatatable.Keyword)) //ad 
            {
                string keyword = requestDatatable.Keyword;
                menus = menus.Where(x => x.Name.Contains(keyword));
            }

            var result = menus.Skip(requestDatatable.SkipItems)
                                    .Take(requestDatatable.PageSize).ToList();

            int totalRecords = menus.Count();

            var data = result.Select(x => new
            {
                x.Id,
                x.Name,
                IsDisplay = $"<input type='checkbox' class='ck-display' {(x.IsDisplay ? "checked" : "" )}></input>",
            }).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = menus.Count(),
                RecordsFiltered = result.Count(),
                Data = data
            };

            return responseDatatable;

        }

        public async Task<ResponseModel> CreateUpdate(MenuViewModel menuViewModel)
        {
            var menu = await _unitOfWork.MenuRepository.GetSingleByConditionAsync(x => x.Name.ToLower() == menuViewModel.Name.ToLower() 
                                                                                   && x.Id != menuViewModel.Id);
            if (menu is not null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Menu name is exist",
                    StatusType = StatusType.Fail,
                    Action = menuViewModel.Id is null ? ActionType.Insert : ActionType.Update
                };
            }

            if (menuViewModel.Id is null)
            {
                var newMenu = new Menu
                {
                    Name = menuViewModel.Name,
                    IsActive = menuViewModel.IsActive
                };

                await _unitOfWork.MenuRepository.Insert(newMenu);
                await _unitOfWork.MenuRepository.Commit();
            }
            else  
            {
                var getMenu = await _unitOfWork.MenuRepository.GetById(menuViewModel.Id);
                getMenu.Name = menuViewModel.Name;
                _unitOfWork.MenuRepository.Update(getMenu);
                await _unitOfWork.MenuRepository.Commit();
            }

            return new ResponseModel
            {
                Status = true,
                Message = menuViewModel.Id is null ? "" : "Insert successful",
                StatusType = StatusType.Success,
                Action = menuViewModel.Id is null ? ActionType.Insert : ActionType.Update
            };
        }

        public async Task DeleteMenu(int key)
        {
            var menu = await _unitOfWork.MenuRepository.GetById(key);
            menu.IsActive = false;
            _unitOfWork.MenuRepository.Update(menu);
            await _unitOfWork.MenuRepository.Commit();
        }

        public async Task UpdateDisplayMenu(UpdateDisplayDTO updateDisplayDTO)
        {
                var menu = await _unitOfWork.MenuRepository.GetById(updateDisplayDTO.key);

                menu.IsDisplay = updateDisplayDTO.IsDisplay;

                _unitOfWork.MenuRepository.Update(menu);

                await _unitOfWork.MenuRepository.Commit();
        }

    }
}
