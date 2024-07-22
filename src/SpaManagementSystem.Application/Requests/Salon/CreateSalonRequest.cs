namespace SpaManagementSystem.Application.Requests.Salon
{
    /// <summary>
    /// Represents a request to create a new salon with the specified details.
    /// </summary>
    public class CreateSalonRequest
    {
        /// <summary>
        /// Gets the name of the salon.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the phone number of the salon.
        /// </summary>
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Gets the email address of the salon.
        /// </summary>
        public string Email { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSalonRequest"/> class with the specified details.
        /// </summary>
        /// <param name="name">The name of the salon.</param>
        /// <param name="phoneNumber">The phone number of the salon.</param>
        /// <param name="email">The email address of the salon.</param>
        public CreateSalonRequest(string name, string phoneNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}