using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon
{
    /// <summary>
    /// Represents a request to update the opening hours of an existing salon.
    /// </summary>
    public class UpdateSalonOpeningHoursRequest
    {
        /// <summary>
        /// Gets or sets the list of opening hours for the salon.
        /// </summary>
        public List<OpeningHoursDto> OpeningHours { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalonOpeningHoursRequest"/> class with the specified opening hours.
        /// </summary>
        /// <param name="openingHours">The list of opening hours to set for the salon.</param>
        public UpdateSalonOpeningHoursRequest(List<OpeningHoursDto> openingHours)
        {
            OpeningHours = openingHours;
        }
    }
}