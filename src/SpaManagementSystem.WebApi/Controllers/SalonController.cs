using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/salon")]
[ApiController]
public class SalonController(ISalonService salonService) : BaseController
{
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSalonRequest request)
    {
        var salon = await salonService.CreateAsync(UserId, request);
        
        return CreatedAtAction(
            actionName: nameof(GetSalonAsync),
            controllerName: "Salon",
            routeValues: new { salonId = salon.Id },
            value: salon
        );
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllSalonsAsync()
    {
        var salons = await salonService.GetAllSalonsByUserIdAsync(UserId);

        return this.OkResponse(salons, "Successfully retrieved salons.");
    }
    
    [Authorize(Roles="Admin, Manager, Employee")]
    [HttpGet("{salonId}")]
    public async Task<IActionResult> GetSalonAsync(Guid salonId)
    {
        var salon = await salonService.GetSalonDetailsByIdAsync(salonId);

        return this.OkResponse(salon, "Successfully retrieved salon.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("{salonId}/manage/update-details")]
    public async Task<IActionResult> UpdateDetailsAsync(Guid salonId, [FromBody] JsonPatchDocument<UpdateSalonRequest> patchDocument)
    {
        var result = await salonService.UpdateSalonAsync(salonId, patchDocument);
        
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{salonId}/manage/opening-hours")]
    public async Task<IActionResult> AddOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        await salonService.AddOpeningHoursAsync(salonId, request);

        return this.OkResponse("Opening hours added successfully.");
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/manage/opening-hours")]
    public async Task<IActionResult> UpdateOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        await salonService.UpdateOpeningHoursAsync(salonId, request);
            
        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpDelete("{salonId}/manage/opening-hours/{dayOfWeek}")]
    public async Task<IActionResult> RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        await salonService.RemoveOpeningHoursAsync(salonId, dayOfWeek);

        return NoContent();
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/manage/address")]
    public async Task<IActionResult> UpdateAddressAsync(Guid salonId, [FromBody] UpdateAddressRequest request)
    {
        await salonService.UpdateAddressAsync(salonId, request);
                
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{salonId}")]
    public async Task<IActionResult> DeleteAsync(Guid salonId)
    {
        await salonService.DeleteAsync(salonId);

        return NoContent();
    }
}