using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/employee")]
public class EmployeeController(IEmployeeService employeeService) : BaseController
{
    /// <summary>
    /// Creates a new employee with the provided details.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin" or "Manager" role to create a new employee in the system.
    /// If the creation process fails due to invalid input, the request will be rejected.
    /// </remarks>
    /// <param name="request">The request object containing the employee's details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the employee creation process.
    /// </returns>
    /// <response code="201">Employee was successfully created in the system.</response>
    /// <response code="400">Returned if the creation failed due to validation errors or incorrect data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to create employees.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await employeeService.AddEmployeeAsync(request);

        return Created();
    }
    
    /// <summary>
    /// Retrieves basic information about the employee associated with the current user.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Manager" or "Employee" role to retrieve their own employee details from the system.
    /// The information is fetched based on the user ID of the currently authenticated user.
    /// </remarks>
    /// <returns>
    /// Returns an HTTP response containing the employee details.
    /// </returns>
    /// <response code="200">Employee details were successfully retrieved.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee details.</response>
    /// <response code="404">Returned if the employee details could not be found for the authenticated user.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Manager, Employee")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeAsync()
    {
        var employee = await employeeService.GetEmployeeByUserId(UserId);

        return Ok(employee);
    }
    
    /// <summary>
    /// Retrieves the details of the employee associated with the current user.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Manager" or "Employee" role to retrieve detailed employee information from the system.
    /// The employee details are fetched based on the user ID of the currently authenticated user.
    /// </remarks>
    /// <returns>
    /// Returns an HTTP response containing detailed employee information.
    /// </returns>
    /// <response code="200">Employee details were successfully retrieved.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee details.</response>
    /// <response code="404">Returned if the employee details could not be found for the authenticated user.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDetailsDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Manager, Employee")]
    [HttpGet("details")]
    public async Task<IActionResult> GetEmployeeDetailsAsync()
    {
        var employee = await employeeService.GetEmployeeDetailsByUserId(UserId);

        return Ok(employee);
    }
}