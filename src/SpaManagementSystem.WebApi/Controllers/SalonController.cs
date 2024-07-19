using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing salons. Provides endpoints for creating, retrieving, updating, and deleting salons.
    /// </summary>
    [Route("api/Salon")]
    [ApiController]
    public class SalonController : BaseController
    {
        private readonly ISalonService _salonService;
        
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SalonController"/> class with the specified <see cref="ISalonService"/>.
        /// </summary>
        /// <param name="salonService">The <see cref="ISalonService"/> instance used to perform salon-related operations.</param>
        public SalonController(ISalonService salonService)
        {
            _salonService = salonService;
        }
        
        
        
        /// <summary>
        /// Creates a new salon with the provided details.
        /// </summary>
        /// <param name="request">The <see cref="CreateSalonRequest"/> object containing the details for the new salon.</param>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSalonRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _salonService.CreateAsync(UserId, request);

            return Created("api/Salon", null);
        }
        
        /// <summary>
        /// Retrieves a list of all salons associated with the current user.
        /// </summary>
        /// <returns>A <see cref="IActionResult"/> containing a JSON result with the list of salons.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAllSalonsAsync()
        {
            var salons = await _salonService.GetAllSalonsByUserIdAsync(UserId);

            return new JsonResult(salons);
        }
        
        /// <summary>
        /// Retrieves details for a specific salon by its ID.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon.</param>
        /// <returns>A <see cref="IActionResult"/> containing the salon details if found, otherwise a NotFound result.</returns>
        [Authorize]
        [HttpGet("{salonId}")]
        public async Task<IActionResult> GetSalonAsync(Guid salonId)
        {
            var salon = await _salonService.GetSalonDetailsByIdAsync(salonId);

            if (salon == null)
                return NotFound($"Salon with id '{salonId}' does not found.");

            return new JsonResult(salon);
        }
        
        /// <summary>
        /// Updates the details of a specific salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to update.</param>
        /// <param name="request">The <see cref="UpdateSalonDetailsRequest"/> object containing the updated salon details.</param>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.</returns>
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
        
        /// <summary>
        /// Updates the opening hours of a specific salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to update.</param>
        /// <param name="request">The <see cref="UpdateSalonOpeningHoursRequest"/> object containing the updated opening hours.</param>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{salonId}/Manage/UpdateOpeningHours")]
        public async Task<IActionResult> UpdateOpeningHoursAsync(Guid salonId, [FromBody] UpdateSalonOpeningHoursRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _salonService.UpdateOpeningHours(salonId, request);
            
            return NoContent();
        }
        
        /// <summary>
        /// Deletes a specific salon by its ID.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to delete.</param>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{salonId}/Manage/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid salonId)
        {
            await _salonService.DeleteAsync(salonId);

            return NoContent();
        }
    }
}