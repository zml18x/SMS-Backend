using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/employee")]
public class EmployeeController(IEmployeeService employeeService, UserManager<User> userManager) : BaseController
{
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return this.BadRequestResponse($"User with ID '{request.UserId}' does not exist.");
        
        var employee = await employeeService.CreateEmployeeAsync(request);

        return CreatedAtAction(
            actionName: nameof(GetEmployeeByIdAsync),
            controllerName: "Employee",
            routeValues: new { employeeId = employee.Employee.Id },
            value: employee
        );
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeAsync()
    {
        var employee = await employeeService.GetEmployeeWithProfileByUserIdAsync(UserId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await employeeService.GetEmployeeWithProfileByIdAsync(employeeId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }

    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllEmployeesAsync(
        [FromQuery] Guid salonId, 
        [FromQuery] string? code = null,
        [FromQuery] string? firstName = null,
        [FromQuery] string? lastName = null,
        [FromQuery] EmploymentStatus? status = null)
    {
        var employees = await employeeService.GetEmployeesAsync(salonId, code, firstName, lastName, status);

        return this.OkResponse(employees, "Successfully retrieved employees.");
    }
    
    [Authorize(Roles ="Admin, Manager, Employee")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] JsonPatchDocument<UpdateEmployeeSelfRequest> patchDocument)
    {
        var result = await employeeService.UpdateEmployeeAsync(UserId, patchDocument);
        
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPatch("details/update")]
    public async Task<IActionResult> UpdateEmployeeProfileAsync([FromBody] JsonPatchDocument<UpdateEmployeeProfileSelfRequest> patchDocument)
    {
        var result = await employeeService.UpdateEmployeeProfileAsync(UserId, patchDocument);
        
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("update/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployeeAsync(
        Guid employeeId,
        [FromBody] JsonPatchDocument<UpdateEmployeeRequest> patchDocument)
    {
        var result = await employeeService.UpdateEmployeeAsync(employeeId, patchDocument);
        
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("details/update/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployeeProfileAsync(
        Guid employeeId,
        [FromBody] JsonPatchDocument<UpdateEmployeeProfileRequest> patchDocument)
    {
        var result = await employeeService.UpdateEmployeeProfileAsync(employeeId, patchDocument);

        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployeeAsync(Guid employeeId)
    {
        await employeeService.DeleteEmployeeAsync(employeeId);
        
        return NoContent();
    }
    
    [HttpPost("{employeeId}/services/{serviceId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> AssignServiceToEmployeeAsync(Guid employeeId, Guid serviceId)
    {
        await employeeService.AssignServiceToEmployeeAsync(employeeId, serviceId);

        return this.OkResponse("Successfully assigned service to employee.");
    }
    
    [HttpDelete("{employeeId}/services/{serviceId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> RemoveServiceFromEmployeeAsync(Guid employeeId, Guid serviceId)
    {
        await employeeService.RemoveServiceFromEmployeeAsync(employeeId, serviceId);

        return NoContent();
    }
    
    [HttpGet("{employeeId}/services")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetEmployeeServicesAsync(Guid employeeId)
    {
        var services = await employeeService.GetEmployeeServices(employeeId);
        
        return this.OkResponse(services, "Successfully retrieved employee services.");
    }
}