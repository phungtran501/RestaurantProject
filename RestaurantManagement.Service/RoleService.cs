using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<SelectListItem> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();

            var result = roles.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,

            });

            return result;
        }

        public async Task<ResponseDatatable> GetListRole(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            var roles = _roleManager.Roles;



            if (!string.IsNullOrEmpty(requestDatatable.Keyword)) //ad 
            {
                string keyword = requestDatatable.Keyword;

                roles = roles.Where(x => x.Name.Contains(keyword));


            }

            int totalRecords = roles.Count();

            var result = await roles.Skip(requestDatatable.SkipItems)
                                    .Take(requestDatatable.PageSize).ToListAsync();

            var data = result.Select(x => new { Id = ActionDatatable(x.Id), x.Name }).ToArray();

            responseDatatable = new ResponseDatatable
            {
                Draw = requestDatatable.Draw,
                RecordsTotal = _roleManager.Roles.Count(),
                RecordsFiltered = result.Count,
                Data = data
            };

            return responseDatatable;
        }

        private string ActionDatatable(string id)
        {
            string delete = "<a href=\"#\" title='delete' class='btn-delete'><span class=\"ti-trash\"></span></a>";
            string edit = $"<a href=\"/admin/role/createupdate?id={id}\" title='edit'><span class=\"ti-pencil\"></span></a>";

            return $"<span data-key=\"{id}\">{edit}&nbsp;{delete}</span>";
        }

        public async Task<ResponseModel> CreateUpdate(RoleViewModel roleViewModel)
        {
            if (string.IsNullOrEmpty(roleViewModel.Id))
            {
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name

                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {

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

                var roles = await  _roleManager.FindByIdAsync(roleViewModel.Id);

                roles.Name = roleViewModel.Name;

                var result = await _roleManager.UpdateAsync(roles);


                if (result.Succeeded)
                {
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
                        Message = result.Errors.ToList()[0].Description,
                        StatusType = StatusType.Fail,
                        Action = ActionType.Update
                    };
                }
            }

        }

        public async Task DeleteRole(string key)
        {
            var role = await _roleManager.FindByIdAsync(key);

            await _roleManager.DeleteAsync(role);

        }
    }
}
