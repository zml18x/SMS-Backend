namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing detailed information about a salon.
    /// </summary>
    public class SalonDetailsDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the salon.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the salon.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the salon.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the salon.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the description of the salon. This property is optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of opening hours for the salon.
        /// </summary>
        public IEnumerable<OpeningHoursDto> OpeningHours { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SalonDetailsDto"/> class with the specified details.
        /// </summary>
        /// <param name="id">The unique identifier of the salon.</param>
        /// <param name="name">The name of the salon.</param>
        /// <param name="email">The email address of the salon.</param>
        /// <param name="phoneNumber">The phone number of the salon.</param>
        /// <param name="description">The description of the salon. Can be null.</param>
        /// <param name="openingHours">The collection of opening hours for the salon.</param>
        public SalonDetailsDto(Guid id, string name, string email, string phoneNumber, string? description,
            IEnumerable<OpeningHoursDto> openingHours)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
            OpeningHours = openingHours;
        }
    }
}