namespace SpaManagementSystem.Application.Requests.Salon
{
    /// <summary>
    /// Represents a request to update the details of an existing salon.
    /// </summary>
    public class UpdateSalonDetailsRequest
    {
        /// <summary>
        /// Gets or sets the name of the salon.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the email address of the salon.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets or sets the phone number of the salon.
        /// </summary>
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Gets or sets the description of the salon.
        /// Optional. Can be null or whitespace.
        /// </summary>
        public string? Description { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalonDetailsRequest"/> class with the specified details.
        /// </summary>
        /// <param name="name">The new name of the salon.</param>
        /// <param name="email">The new email address of the salon.</param>
        /// <param name="phoneNumber">The new phone number of the salon.</param>
        /// <param name="description">Optional. The new description of the salon.</param>
        public UpdateSalonDetailsRequest(string name, string email, string phoneNumber, string? description)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
        }
    }
}