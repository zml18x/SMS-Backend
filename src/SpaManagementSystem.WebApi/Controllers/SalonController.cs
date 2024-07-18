using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.WebApi.Controllers
{
    [Route("api/Salon")]
    [ApiController]
    public class SalonController : ControllerBase
    {
        private Guid UserId => User.Identity!.IsAuthenticated ? Guid.Parse(User.Identity.Name!) : Guid.Empty;
        private readonly ISalonService _salonService;
        
        
        
        public SalonController(ISalonService salonService)
        {
            _salonService = salonService;
        }
        
        
        
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSalonRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _salonService.CreateAsync(UserId, request);

            return Created("api/Salon", null);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAllSalonsAsync()
        {
            var salons = await _salonService.GetAllSalonsByUserIdAsync(UserId);

            return new JsonResult(salons);
        }

        [Authorize]
        [HttpGet("{salonId}")]
        public async Task<IActionResult> GetSalonAsync(Guid salonId)
        {
            var salon = await _salonService.GetSalonDetailsByIdAsync(salonId);

            if (salon == null)
                return NotFound($"Salon with id '{salonId}' does not found.");

            return new JsonResult(salon);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{salonId}/Manage/UpdateDetails")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid salonId, [FromBody] UpdateSalonDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isUpdated = await _salonService.UpdateSalonAsync(salonId, request);

            if (!isUpdated)
                return BadRequest("No changes were made to the salon.");

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{salonId}/Manage/UpdateOpeningHours")]
        public async Task<IActionResult> UpdateOpeningHoursAsync(Guid salonId, [FromBody] UpdateSalonOpeningHoursRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _salonService.UpdateOpeningHours(salonId, request);
            
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{salonId}/Manage/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid salonId)
        {
            await _salonService.DeleteAsync(salonId);

            return NoContent();
        }
    }
}