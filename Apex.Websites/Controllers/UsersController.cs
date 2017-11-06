// using Apex.Data.Entities.Accounts;
// using Apex.Data.Paginations;
// using Apex.Services.Logs;
// using Apex.Websites.ViewModels.Users;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System;

// namespace Apex.Websites.Controllers
// {
//     public class UsersController : BaseAdminController
//     {
//         private readonly RoleManager<ApplicationRole> _roleManager;
//         private readonly UserManager<ApplicationUser> _userManager;
//         private readonly IActivityLogService _activityLogService;

//         public UsersController(
//             RoleManager<ApplicationRole> roleManager,
//             UserManager<ApplicationUser> userManager,
//             IActivityLogService activityLogService)
//         {
//             _roleManager = roleManager;
//             _userManager = userManager;
//             _activityLogService = activityLogService;
//         }

//         [HttpGet("{id}")]
//         public async Task<UserDto> Get(int id)
//         {
//             var user = await FindAsync(id);

//             if (user == null)
//             {
//                 return null;
//             }

//             var roleNames = await _userManager.GetRolesAsync(user);

//             return new UserDto(user, roleNames);
//         }

//         [HttpGet()]
//         public IPagedList<UserDto> Get(
//             string email,
//             int[] roleIds,
//             int page,
//             int size)
//         {
//             var query = _userManager.Users
//                 .Include(u => u.Roles)
//                 .AsNoTracking();

//             if (!string.IsNullOrWhiteSpace(email))
//             {
//                 query = query.Where(u => u.Email.StartsWith(email));
//             }

//             // if (roleIds != null && roleIds.Length > 0)
//             // {
//             //     query = query.Where(u => roleIds.Contains(u.Role));
//             // }

//             query = query.OrderBy(u => u.Id);

//             return new PagedList<UserDto>(query.Select(u => new UserDto(u)), page, size);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Post([FromBody]CreateUserViewModel viewModel)
//         {
//             if (ModelState.IsValid)
//             {
//                 var applicationUser = viewModel.ToApplicationUser();
//                 applicationUser.EmailConfirmed = true;
//                 var result = await _userManager.CreateAsync(applicationUser, viewModel.Password);

//                 if (result.Succeeded && viewModel.Locked)
//                 {
//                     result = await LockUserAsync(applicationUser);
//                 }

//                 if (result.Succeeded && viewModel.RoleNames != null && viewModel.RoleNames.Length != 0)
//                 {
//                     result = await _userManager.AddToRolesAsync(applicationUser, viewModel.RoleNames);
//                 }

//                 if (result.Succeeded)
//                 {
//                     var activityLog = GetActivityLog(applicationUser.GetType(), newValue: applicationUser);
//                     await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

//                     return Created("Post", new UserDto(applicationUser));
//                 }

//                 AddErrorsToModelState(result);
//             }

//             return BadRequest(ModelState);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Put(int id, [FromBody]UpdateUserViewModel viewModel)
//         {
//             if (ModelState.IsValid)
//             {
//                 var applicationUser = await FindAsync(id);

//                 if (applicationUser == null)
//                 {
//                     return NotFound();
//                 }

//                 var activityLog = GetActivityLog(applicationUser.GetType(), oldValue: applicationUser);

//                 var updatedApplicationUser = viewModel.ToApplicationUser(applicationUser);
//                 var result = await _userManager.UpdateAsync(updatedApplicationUser);
//                 var isLockedOut = await _userManager.IsLockedOutAsync(updatedApplicationUser);

//                 if (viewModel.Locked && !isLockedOut)
//                 {
//                     result = await LockUserAsync(updatedApplicationUser);
//                 }
//                 else if (!viewModel.Locked && isLockedOut)
//                 {
//                     result = await UnlockUserAsync(updatedApplicationUser);
//                 }

//                 // Assign Roles.
//                 result = await AssignRolesToUserAsync(updatedApplicationUser, viewModel.RoleNames);

//                 if (result.Succeeded)
//                 {
//                     activityLog.NewValue = ObjectToJson(updatedApplicationUser);
//                     await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

//                     return NoContent();
//                 }

//                 AddErrorsToModelState(result);
//             }

//             return BadRequest(ModelState);
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(int id)
//         {
//             var applicationUser = await FindAsync(id);

//             if (applicationUser == null)
//             {
//                 return NotFound();
//             }

//             var result = await _userManager.DeleteAsync(applicationUser);

//             if (result.Succeeded)
//             {
//                 var activityLog = GetActivityLog(applicationUser.GetType(), newValue: applicationUser);
//                 await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

//                 return NoContent();
//             }

//             AddErrorsToModelState(result);

//             return BadRequest(ModelState);
//         }

//         [HttpPut("{id}/ResetPassword")]
//         public async Task<IActionResult> ResetPassword(int id, [FromBody]ResetUserPasswordViewModel viewModel)
//         {
//             if (ModelState.IsValid)
//             {
//                 var applicationUser = await FindAsync(id);

//                 if (applicationUser == null)
//                 {
//                     return NotFound();
//                 }

//                 foreach (var validator in _userManager.PasswordValidators)
//                 {
//                     var pwdResult = await validator.ValidateAsync(_userManager, applicationUser, viewModel.Password);

//                     if (!pwdResult.Succeeded)
//                     {
//                         AddErrorsToModelState(pwdResult);

//                         return BadRequest(ModelState);
//                     }
//                 }

//                 var result = await _userManager.RemovePasswordAsync(applicationUser);

//                 if (result.Succeeded)
//                 {
//                     result = await _userManager.AddPasswordAsync(applicationUser, viewModel.Password);

//                     if (result.Succeeded)
//                     {
//                         var activityLog = GetActivityLog(applicationUser.GetType());
//                         await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

//                         return NoContent();
//                     }
//                 }

//                 AddErrorsToModelState(result);
//             }

//             return BadRequest(ModelState);
//         }

//         private async Task<ApplicationUser> FindAsync(int id)
//         {
//             return id <= 0 ?
//                 null :
//                 await _userManager.FindByIdAsync(id.ToString());
//         }

//         private async Task<IdentityResult> LockUserAsync(ApplicationUser entity)
//         {
//             return await _userManager.SetLockoutEndDateAsync(entity, DateTimeOffset.MaxValue);
//         }

//         private async Task<IdentityResult> UnlockUserAsync(ApplicationUser entity)
//         {
//             var result = await _userManager.SetLockoutEndDateAsync(entity, null);

//             if (result.Succeeded)
//             {
//                 result = await _userManager.ResetAccessFailedCountAsync(entity);
//             }

//             return result;
//         }

//         private async Task<IdentityResult> AssignRolesToUserAsync(ApplicationUser entity, string[] roleNames)
//         {
//             IdentityResult result = IdentityResult.Success;
//             var currentRoles = await _userManager.GetRolesAsync(entity);

//             if (roleNames == null || roleNames.Length == 0)
//             {
//                 result = await _userManager.RemoveFromRolesAsync(entity, currentRoles);
//             }
//             else
//             {
//                 var rolesExists = roleNames.Intersect(_roleManager.Roles.Select(r => r.Name));

//                 if (rolesExists.Any())
//                 {
//                     result = await _userManager.RemoveFromRolesAsync(entity, currentRoles);

//                     if (result.Succeeded)
//                     {
//                         result = await _userManager.AddToRolesAsync(entity, rolesExists);
//                     }
//                 }
//             }

//             return result;
//         }

//         private void AddErrorsToModelState(IdentityResult result)
//         {
//             foreach (var error in result.Errors)
//             {
//                 ModelState.AddModelError(string.Empty, error.Description);
//             }
//         }
//     }
// }