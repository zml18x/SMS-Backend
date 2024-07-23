using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Address;
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
        /// <remarks>
        /// This endpoint allows an authenticated admin to create a new salon by providing the necessary details in the request body.
        /// The request should include details such as the salon's name, address, and other relevant information. Upon successful 
        /// creation of the salon, a `201 Created` response is returned.
        ///
        /// Sample request:
        /// 
        ///     POST /Salon/Create
        ///     {
        ///         "name": "ExampleName",
        ///         "phoneNumber": "321321123"
        ///         "email": "example@mail.com"
        ///     }
        /// </remarks>
        /// <param name="request">The CreateSalonRequest object containing the details for the new salon.</param>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
        /// <response code="201">Indicates that the salon was created successfully.</response>
        /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
        /// <response code="401">If the user is not authenticated (authorization failed).</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        [Produces("application/json")]
        [Consumes("application/json")]
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
        /// <remarks>
        /// This endpoint allows an authenticated admin to retrieve a list of all salons that are associated with the current user.
        /// The request does not require any parameters. The response will contain a list of salon objects in JSON format.
        /// 
        /// Sample request:
        /// 
        ///     GET /Salon/List
        /// 
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> containing a JSON result with the list of salons.</returns>
        /// <response code="200">Returns a list of salons associated with the current user.</response>
        /// <response code="401">If the user is not authenticated (authorization failed).</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<SalonDto>), StatusCodes.Status200OK)]
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
        /// <remarks>
        /// This endpoint retrieves detailed information about a specific salon using its unique identifier (ID). 
        /// If the salon with the specified ID is found, the details are returned in JSON format.
        /// If no salon is found, a `404 Not Found` response is returned.
        /// 
        /// Sample request:
        /// 
        ///     GET /Salon/{salonId}
        /// 
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> containing the salon details if found, otherwise a NotFound result.</returns>
        /// <response code="200">Returns the details of the salon if found.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        /// <response code="401">If the request is not authenticated.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(SalonDetailsDto), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{salonId}")]
        public async Task<IActionResult> GetSalonAsync(Guid salonId)
        {
            var salon = await _salonService.GetSalonDetailsByIdAsync(salonId);

            return new JsonResult(salon);
        }

        
        /// <summary>
        /// Updates the details of a specific salon based on the provided JsonPatchDocument.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to update.</param>
        /// <param name="patchDocument">The JsonPatchDocument object containing the details to update.</param>
        /// <param name="requestValidator">An instance of <see cref="IValidator{UpdateSalonDetailsRequest}"/> used to validate the updated salon details.</param>
        /// <remarks>
        /// This endpoint allows you to update specific details of a salon using a JSON patch document.
        /// The patch document should contain the operations to apply to the salon details.
        ///
        /// Sample request:
        /// 
        ///     PATCH /Salon/{salonId}/Manage/UpdateDetails
        ///     [
        ///         { "op": "replace", "path": "/name", "value": "New Salon Name" },
        ///         { "op": "replace", "path": "/phoneNumber", "value": "321321111" },
        ///         { "op": "replace", "path": "/email", "value": "newExample@mail.com" },
        ///         { "op": "replace", "path": "/description", "value": "Example description" }
        ///     ]
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.
        /// Returns <see cref="NotFoundResult"/> if the salon is not found,
        /// <see cref="BadRequestResult"/> if the request is invalid,
        /// or <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update was successful and no content is returned.</response>
        /// <response code="400">If the request is invalid or no changes were made to the salon.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        [Produces("application/json-patch+json")]
        [Authorize(Roles = "Admin")]
        [HttpPatch("{salonId}/Manage/UpdateDetails")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid salonId, [FromBody] JsonPatchDocument<UpdateSalonDetailsRequest> patchDocument,
            [FromServices] IValidator<UpdateSalonDetailsRequest> requestValidator)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var existingSalon = await _salonService.GetSalonDetailsByIdAsync(salonId);

            var request = new UpdateSalonDetailsRequest(existingSalon.Name, existingSalon.Email,
                existingSalon.PhoneNumber, existingSalon.Description);

            patchDocument.ApplyTo(request, ModelState);

            var requestValidationResult = await requestValidator.ValidateAsync(request);
            if (!requestValidationResult.IsValid)
            {
                foreach (var error in requestValidationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }

            var isUpdated = await _salonService.UpdateSalonAsync(salonId, request);
            if (!isUpdated)
                return BadRequest("No changes were made to the salon.");

            return NoContent();
        }

        /// <summary>
        /// Updates the opening hours of a specific salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to update.</param>
        /// <param name="request">The UpdateSalonOpeningHoursRequest object containing the updated opening hours.</param>
        /// <remarks>
        /// This endpoint allows you to update the opening hours for a specific salon. The request body should
        /// contain the opening hours in the format specified below.
        ///
        /// Sample request:
        /// 
        ///     PUT /Salon/{salonId}/Manage/UpdateOpeningHours
        ///     {
        ///         "openingHours": [
        ///             {
        ///                 "dayOfWeek": 0, // 0 for Sunday, 1 for Monday, ..., 6 for Saturday
        ///                 "openingTime": "09:00:00",
        ///                 "closingTime": "18:00:00",
        ///                 "isClosed": false
        ///             },
        ///             {
        ///                 "dayOfWeek": 1,
        ///                 "openingTime": "09:00:00",
        ///                 "closingTime": "18:00:00",
        ///                 "isClosed": true
        ///             },
        ///             // ... other days
        ///         ]
        ///     }
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.
        /// Returns <see cref="NoContentResult"/> if the update is successful,
        /// or <see cref="BadRequestResult"/> if the request is invalid.</returns>
        /// <response code="204">If the update was successful and no content is returned.</response>
        /// <response code="400">If the request is invalid or if there is an error updating the opening hours.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        [Consumes("application/json")]
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
        /// Creates a new address for a specific salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to which the address will be added.</param>
        /// <param name="request">The CreateAddressRequest object containing the address details.</param>
        /// <remarks>
        /// This endpoint allows an admin to create a new address for a specific salon. The request body should
        /// contain the details of the address to be added.
        ///
        /// Sample request:
        /// 
        ///     POST /Salon/{salonId}/Manage/Address/Create
        ///     {
        ///         "country": "Poland",
        ///         "region": "Małopolskie",
        ///         "city": "Zakopane",
        ///         "postalCode": "34-500",
        ///         "street": "Ulica",
        ///         "buildingNumber": "10x"
        ///     }
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the address creation operation.
        /// Returns <see cref="OkResult"/> if the address was created successfully, or <see cref="BadRequestResult"/> if the request is invalid.</returns>
        /// <response code="200">If the address was created successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        [HttpPost("{salonId}/Manage/Address/Create")]
        public async Task<IActionResult> CreateSalonAddressAsync(Guid salonId, [FromBody] CreateAddressRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _salonService.AddAddress(salonId, request);
                
            return Ok();
        }
        
        /// <summary>
        /// Updates the address of a specific salon based on the provided JsonPatchDocument/>.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon whose address will be updated.</param>
        /// <param name="patchDocument">The JsonPatchDocument object containing the details to update.</param>
        /// <param name="requestValidator">An instance of <see cref="IValidator{UpdateAddressRequest}"/> used to validate the updated address details.</param>
        /// <remarks>
        /// This endpoint allows an admin to update the address of a specific salon using a JSON Patch document. 
        /// The request body should contain the changes to be applied to the salon's address.
        ///
        /// Sample request:
        /// 
        ///     PATCH /Salon/{salonId}/Manage/Address/Update
        ///     [
        ///         { "op": "replace", "path": "/country", "value": "Polska" },
        ///         { "op": "replace", "path": "/region", "value": "Małopolskie" }
        ///         { "op": "replace", "path": "/city", "value": "Zakopane" },
        ///         { "op": "replace", "path": "/postalCode", "value": "34-500" }
        ///         { "op": "replace", "path": "/street", "value": "Ulica" },
        ///         { "op": "replace", "path": "/buildingNumber", "value": "100x" }
        ///     ]
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the address update operation.
        /// Returns <see cref="BadRequestResult"/> if the address does not exist or the request is invalid,
        /// or <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the address was updated successfully.</response>
        /// <response code="400">If the request is invalid or the address does not exist.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        [HttpPatch("{salonId}/Manage/Address/Update")]
        public async Task<IActionResult> UpdateSalonAddressAsync(Guid salonId, JsonPatchDocument<UpdateAddressRequest> patchDocument,
            [FromServices] IValidator<UpdateAddressRequest> requestValidator)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var salon = await _salonService.GetSalonDetailsByIdAsync(salonId);
            var address = salon.Address;

            if (address == null)
                return BadRequest($"Address for salon with ID '{salonId}' does not exist");
            
            var request = new UpdateAddressRequest(address.Country, address.Region, address.City, address.PostalCode,
                address.Street, address.BuildingNumber);

            patchDocument.ApplyTo(request);
            
            var requestValidationResult = await requestValidator.ValidateAsync(request);
            if (!requestValidationResult.IsValid)
            {
                foreach (var error in requestValidationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return BadRequest(ModelState);
            }
            
            var isUpdated = await _salonService.UpdateAddress(salonId, request);
            if (!isUpdated)
                return BadRequest("No changes were made to the salon address.");

            return NoContent();
        }
        
        /// <summary>
        /// Deletes a specific salon by its ID.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to delete.</param>
        /// <remarks>
        /// This endpoint allows an admin to delete a specific salon using its unique identifier. 
        /// If the salon is successfully deleted, no content will be returned.
        ///
        /// Sample request:
        /// 
        ///     DELETE /Salon/{salonId}/Manage/Delete
        /// 
        /// </remarks>
        /// <returns>A <see cref="IActionResult"/> indicating the result of the delete operation.
        /// Returns <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the salon was deleted successfully.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
        /// <response code="404">If the salon with the specified ID is not found.</response>
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{salonId}/Manage/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid salonId)
        {
            await _salonService.DeleteAsync(salonId);

            return NoContent();
        }
    }
}