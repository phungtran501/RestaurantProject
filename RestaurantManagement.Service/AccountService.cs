using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using RestaurantManagement.Data;
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
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageHandler _imageHandler;
        private readonly IRoleService _roleService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IDapperHelper _dapperHelper;

        public AccountService(UserManager<ApplicationUser> userManager, 
                                    IImageHandler imageHandler,
                                    IRoleService roleService,
                                    RoleManager<IdentityRole> roleManager,
                                    IConfiguration configuration,
                                    IDapperHelper dapperHelper)
        {
            _userManager = userManager;
            _imageHandler = imageHandler;
            _roleService = roleService;
            _roleManager = roleManager;
            _configuration = configuration;
            _dapperHelper = dapperHelper;
        }

        public async Task<ResponseDatatable> GetListAccount(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            IQueryable<ApplicationUser> users = _userManager.Users.Where(x => x.IsActive && x.UserName != "Administrator");  //get all uses in db

            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("skipItem", requestDatatable.SkipItems, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("pageSize", requestDatatable.PageSize, DbType.Int32, ParameterDirection.Input);
            dynamicParameters.Add("keyWord", requestDatatable.Keyword, DbType.String, ParameterDirection.Input);
            dynamicParameters.Add("totalRecord", 0, DbType.Int32, ParameterDirection.Output);

            var result = await _dapperHelper.ExcuteStoreProcedureReturnList<UserModel>("GetAllUsers", dynamicParameters);

            var total = dynamicParameters.Get<int>("totalRecord");


            var data = result.Select(x => new { Id = ActionDatatable(x.Id), 
                                                x.Username, 
                                                x.RoleName, 
                                                x.Fullname, 
                                                x.Email, 
                                                x.PhoneNumber, 
                                                x.Address,
                                                IsSystem = x.IsSystem ? "Yes" : "No",
                                                IsActive = x.IsActive ? "Yes" : "No"
            })
                            .ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = _userManager.Users.Count(),
                RecordsFiltered = result.Count(),
                Data = data
            };

            return responseDatatable;
        }

        private string ActionDatatable(string id)
        {
            string delete = "<a href=\"#\" title='delete' class='btn-delete'><span class=\"ti-trash\"></span></a>";
            string edit = $"<a href=\"/admin/account/createupdate?id={id}\" title='edit'><span class=\"ti-pencil\"></span></a>";

            return $"<span data-key=\"{id}\">{edit}&nbsp;{delete}</span>";
        }

        public async Task<ResponseModel> CreateUpdate(AccountViewModel accountViewModel)
        {
            if (string.IsNullOrEmpty(accountViewModel.Id))
            {
                var user = new ApplicationUser
                {
                    UserName = accountViewModel.Username,
                    Email = accountViewModel.Email,
                    PhoneNumber = accountViewModel.PhoneNumber,
                    Fullname = accountViewModel.Fullname,
                    Address = accountViewModel.Address,
                    IsActive = accountViewModel.IsActive,
                    IsSystem = accountViewModel.IsSystem,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0

                };




                var result = await _userManager.CreateAsync(user, accountViewModel.Password);

                if (result.Succeeded)
                {
                    var roles = await _userManager.AddToRoleAsync(user, accountViewModel.RoleId);

                    await _imageHandler.SaveImage("wwwroot/images/account", new List<IFormFile> { accountViewModel.Avatar }, $"{user.Id}.png");

                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Insert successful",
                        StatusType = StatusType.Success,
                        Action = ActionType.Insert
                    };
                }
                else
                {
                    var errors = result.Errors.ToList().Select(x => x.Description);

                    return new ResponseModel
                    {
                        Status = false,
                        Message = string.Join(';', errors),
                        StatusType = StatusType.Fail,
                        Action = ActionType.Insert
                    };
                }
            }
            //update
            else
            {
                // 1 user --> 1 role (check create/update)
                var user = await _userManager.FindByIdAsync(accountViewModel.Id);

                var roles = await _userManager.GetRolesAsync(user);
                

                var isExist = await _userManager.IsInRoleAsync(user, accountViewModel.RoleId);

                if(!isExist)
                {
                    await _userManager.RemoveFromRoleAsync(user, accountViewModel.RoleId);
                    await _userManager.AddToRoleAsync(user, accountViewModel.RoleId);
                }
                

                user.Address = accountViewModel.Address;
                user.Fullname = accountViewModel.Fullname;
                user.Email = accountViewModel.Email;
                user.Address = accountViewModel.Address;
                user.IsActive = accountViewModel.IsActive;
                user.IsSystem = accountViewModel.IsSystem;
                user.PhoneNumber = accountViewModel.PhoneNumber;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _imageHandler.SaveImage("wwwroot/images/account", new List<IFormFile> { accountViewModel.Avatar }, $"{user.Id}.png");
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Update successful",
                        StatusType = StatusType.Success,
                        Action = ActionType.Update
                    };
                }
                else
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Update failed",
                        StatusType = StatusType.Fail,
                        Action = ActionType.Update
                    };
                }
            }

            
        }

        public async Task DeleteUser(string key)
        {
            var user = await _userManager.FindByIdAsync(key);
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task<AccountViewModel> GetInfomationUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = new AccountViewModel
            {
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            };

            return result;

        }

        public IEnumerable<SelectListItem> GetUser()
        {
            IQueryable<ApplicationUser> user = _userManager.Users.Where(x => x.IsActive);

            var result = user.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName,

            });

            return result;
        }
    }
}
