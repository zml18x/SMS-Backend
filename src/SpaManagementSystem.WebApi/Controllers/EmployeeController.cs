using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Employee.Validators;

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
        var employee = await employeeService.GetEmployeeByUserIdAsync(UserId);

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
        var employee = await employeeService.GetEmployeeDetailsByUserIdAsync(UserId);

        return Ok(employee);
    }
    
    /// <summary>
    /// Retrieves the employee information by the specified employee ID.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin" or "Manager" role to retrieve basic employee information
    /// based on the unique identifier of the employee.
    /// </remarks>
    /// <param name="employeeId">The unique identifier of the employee.</param>
    /// <returns>
    /// Returns an HTTP response containing the employee information.
    /// </returns>
    /// <response code="200">Employee information was successfully retrieved.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee information.</response>
    /// <response code="404">Returned if the employee could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("get-by-id/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(employeeId);

        return Ok(employee);
    }
    
    /// <summary>
    /// Retrieves detailed information of the employee by the specified employee ID.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin" or "Manager" role to retrieve detailed employee information
    /// based on the unique identifier of the employee.
    /// </remarks>
    /// <param name="employeeId">The unique identifier of the employee.</param>
    /// <returns>
    /// Returns an HTTP response containing detailed employee information.
    /// </returns>
    /// <response code="200">Detailed employee information was successfully retrieved.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee details.</response>
    /// <response code="404">Returned if the employee details could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("get-details-by-id/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeDetailsByIdAsync(Guid employeeId)
    {
        var employee = await employeeService.GetEmployeeDetailsByIdAsync(employeeId);

        return Ok(employee);
    }
    
    /// <summary>
    /// Retrieves the employee information by the specified employee code.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin", "Manager", or "Employee" role to retrieve basic employee information
    /// based on the unique employee code.
    /// </remarks>
    /// <param name="employeeCode">The unique code of the employee.</param>
    /// <returns>
    /// Returns an HTTP response containing the employee information.
    /// </returns>
    /// <response code="200">Employee information was successfully retrieved.</response>
    /// <response code="400">Returned if the employee code is not valid.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee information.</response>
    /// <response code="404">Returned if the employee could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("get-by-code/{employeeCode}")]
    public async Task<IActionResult> GetEmployeeByCodeAsync(string employeeCode)
    {
        var validationResult = await new EmployeeCodeValidator().ValidateAsync(employeeCode);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var employee = await employeeService.GetEmployeeByCodeAsync(employeeCode);

        return Ok(employee);
    }
    
    
    /// <summary>
    /// Retrieves detailed information of the employee by the specified employee code.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin", "Manager", or "Employee" role to retrieve detailed employee information
    /// based on the unique employee code.
    /// </remarks>
    /// <param name="employeeCode">The unique code of the employee.</param>
    /// <returns>
    /// Returns an HTTP response containing detailed employee information.
    /// </returns>
    /// <response code="200">Detailed employee information was successfully retrieved.</response>
    /// <response code="400">Returned if the employee code is not valid.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view employee details.</response>
    /// <response code="404">Returned if the employee details could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [ProducesResponseType(typeof(EmployeeDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("get-details-by-code/{employeeCode}")]
    public async Task<IActionResult> GetEmployeeDetailsByCodeAsync(string employeeCode)
    {
        var validationResult = await new EmployeeCodeValidator().ValidateAsync(employeeCode);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var employee = await employeeService.GetEmployeeDetailsByCodeAsync(employeeCode);

        return Ok(employee);
    }
}