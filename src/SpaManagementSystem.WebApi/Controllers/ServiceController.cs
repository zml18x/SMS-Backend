using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Service;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/service")]
public class ServiceController(ISalonServiceService serviceService) : BaseController
{
    [HttpPost("create")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateServiceAsync([FromBody] CreateServiceRequest request)
    {
        var service = await serviceService.CreateServiceAsync(request);
        
        return CreatedAtAction(
            actionName: nameof(GetByIdAsync),
            controllerName: "Service",
            routeValues: new { serviceId = service.Id },
            value: service
        );
    }
    
    [HttpGet("{serviceId}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetByIdAsync(Guid serviceId)
    {
        var service = await serviceService.GetServiceByIdAsync(serviceId);

        return this.OkResponse(service, "Successful retrieved service.");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetServicesAsync([FromQuery] Guid salonId, [FromQuery] string? code = null,
        [FromQuery] string? name = null, [FromQuery] bool? active = null)
    {
        var services = await serviceService.GetServicesAsync(salonId, code, name, active);

        return this.OkResponse(services, "Successful retrieved services.");
    }

    [HttpPatch("{serviceId:guid}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> UpdateServiceAsync(Guid serviceId, 
        [FromBody] JsonPatchDocument<UpdateServiceRequest> patchDocument)
    {
        var result = await serviceService.UpdateServiceAsync(serviceId, patchDocument);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }

    [HttpDelete("{serviceId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteServiceAsync(Guid serviceId)
    {
        await serviceService.DeleteAsync(serviceId);

        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpGet("{serviceId}/employees")]
    public async Task<IActionResult> GetEmployeesByServiceAsync(Guid serviceId)
    {
        var employees = await serviceService.GetEmployeesAssignedToServiceAsync(serviceId);
        
        return this.OkResponse(employees, "Successful retrieved employees.");
    }
}