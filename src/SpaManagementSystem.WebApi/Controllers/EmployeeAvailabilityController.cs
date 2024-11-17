using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.EmployeeAvailability;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("/api/employee-availability")]
public class EmployeeAvailabilityController(IEmployeeAvailabilityService availabilityService) : BaseController
{
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{employeeId}")]
    public async Task<IActionResult> CreateAvailabilitiesForEmployeeAsync(
        Guid employeeId,
        [FromBody] CreateAvailabilityRequest request)
    {
        var availabilities = await availabilityService.CreateAvailabilitiesByEmployeeIdAsync(employeeId, request);

        return CreatedAtAction(
            nameof(GetAvailabilitiesAsync),
            new { employeeId },
            availabilities
        );
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPost("self")]
    public async Task<IActionResult> CreateSelfAvailabilitiesAsync([FromBody] CreateAvailabilityRequest request)
    {
        var availabilities = await availabilityService.CreateAvailabilitiesByUserIdAsync(UserId, request);

        var employeeId = availabilities.First().EmployeeId;
        
        return CreatedAtAction(
            nameof(GetAvailabilitiesAsync),
            new { employeeId },
            availabilities
        );
    }

    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("{employeeId:guid}")]
    public async Task<IActionResult> GetAvailabilitiesAsync(
        Guid employeeId,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null)
    {
        var availabilities = await availabilityService.GetAvailabilitiesAsync(employeeId, startDate, endDate);

        return this.OkResponse(availabilities, "Availabilities retrieved successfully.");
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPut("{availabilityId:guid}")]
    public async Task<IActionResult> UpdateAvailabilityAsync(
        Guid availabilityId,
        [FromBody] UpdateAvailabilityRequest request)
    {
        await availabilityService.UpdateAvailabilityAsync(availabilityId, request);
        
        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpDelete("availability/{availabilityId:guid}")]
    public async Task<IActionResult> DeleteAvailabilityAsync(Guid availabilityId)
    {
        await availabilityService.DeleteAvailabilityAsync(availabilityId);
        
        return NoContent();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteAvailabilitiesForEmployeeAsync(
        Guid employeeId,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        await availabilityService.DeleteAvailabilitiesInRangeForEmployeeAsync(employeeId, startDate, endDate);

        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpDelete("self")]
    public async Task<IActionResult> DeleteAvailabilitiesAsync(
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        await availabilityService.DeleteAvailabilitiesInRangeByUserIdAsync(UserId, startDate, endDate);

        return NoContent();
    }
}