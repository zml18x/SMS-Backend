using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Employee.Validators;
using SpaManagementSystem.WebApi.Extensions;
using SpaManagementSystem.WebApi.Models;

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
        
        var employee = await employeeService.AddEmployeeAsync(request);

        return CreatedAtAction(
            actionName: nameof(GetEmployeeDetailsAsync),
            controllerName: "Employee",
            routeValues: new { employee = employee.Employee.Id },
            value: employee
        );
    }
    
    [Authorize(Roles = "Manager, Employee")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeAsync()
    {
        var employee = await employeeService.GetEmployeeByUserIdAsync(UserId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize(Roles = "Manager, Employee")]
    [HttpGet("details")]
    public async Task<IActionResult> GetEmployeeDetailsAsync()
    {
        var employee = await employeeService.GetEmployeeDetailsByUserIdAsync(UserId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("get-by-id/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(employeeId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("get-details-by-id/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeDetailsByIdAsync(Guid employeeId)
    {
        var employee = await employeeService.GetEmployeeDetailsByIdAsync(employeeId);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("get-by-code/{employeeCode}")]
    public async Task<IActionResult> GetEmployeeByCodeAsync(string employeeCode)
    {
        var validationResult = await new EmployeeCodeValidator().ValidateAsync(employeeCode);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToDictionary(error => error.ErrorCode, error => new[] { error.ErrorMessage });
        
            return BadRequest(new ValidationErrorResponse { Errors = errors });
        }
        
        var employee = await employeeService.GetEmployeeByCodeAsync(employeeCode);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("get-details-by-code/{employeeCode}")]
    public async Task<IActionResult> GetEmployeeDetailsByCodeAsync(string employeeCode)
    {
        var validationResult = await new EmployeeCodeValidator().ValidateAsync(employeeCode);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var employee = await employeeService.GetEmployeeDetailsByCodeAsync(employeeCode);

        return this.OkResponse(employee, "Successfully retrieved employee.");
    }
    
    [Authorize("Manager, Employee")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateEmployeeAsync()
    {
        throw new NotImplementedException();
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("update/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployeeAsync(Guid employeeId, [FromBody] JsonPatchDocument<UpdateEmployeeRequest> patchDocument,
        [FromServices] IValidator<UpdateEmployeeRequest> requestValidator)
    {
        var existingEmployee = await employeeService.GetEmployeeByIdAsync(employeeId);

        var request = new UpdateEmployeeRequest(existingEmployee.Position, existingEmployee.EmploymentStatus,
            existingEmployee.Code, existingEmployee.Color, existingEmployee.HireDate, existingEmployee.Notes);
        
        patchDocument.ApplyTo(request, ModelState);

        if (!employeeService.HasChanges(existingEmployee, request))
            return NoContent();

        var requestValidationResult = await requestValidator.ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            var errors = requestValidationResult.Errors
                .ToDictionary(error => error.PropertyName, error => new[] { error.ErrorMessage });
        
            return BadRequest(new ValidationErrorResponse { Errors = errors });
        }
        
        await employeeService.UpdateEmployee(employeeId, request);

        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("details/update/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployeeDetailsAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }
}