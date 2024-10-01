﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(UserManager<User> userManager) : BaseController
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserAsync()
    {
        var user = await userManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return this.BadRequestResponse("User not found. Please check the user ID.");
        
        var userRoles = await userManager.GetRolesAsync(user);
        var userDto = new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles);
        
        return this.OkResponse<object>(userDto, "User retrieved successfully.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return this.BadRequestResponse("User not found. Please check the user ID.");
        
        var userRoles = await userManager.GetRolesAsync(user);
        var userDto = new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles);

        return this.OkResponse<object>(userDto, "User retrieved successfully.");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("assign-role/{userId:guid}")]
    public async Task<IActionResult> AssignManagerRoleAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return this.BadRequestResponse("User not found. Please check the user ID.");

        var result = await userManager.AddToRoleAsync(user, RoleTypes.Manager.ToString());
        if (!result.Succeeded)
            return this.BadRequestResponse("Error while assigning the role.");

        return this.OkResponse("Manager role has been assigned.");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("remove-role/{userId:guid}")]
    public async Task<IActionResult> RemoveManagerRoleAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return this.BadRequestResponse("User not found. Please check the user ID.");

        var result = await userManager.RemoveFromRoleAsync(user, RoleTypes.Manager.ToString());
        if (!result.Succeeded)
            return this.BadRequestResponse("Error while removing the role.");
        
        return this.OkResponse("Manager role has been removed.");
    }
}