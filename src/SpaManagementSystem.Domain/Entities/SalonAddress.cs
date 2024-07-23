namespace SpaManagementSystem.Domain.Entities
{
    /// <summary>
    /// Represents a salon within the Spa Management System.
    /// Inherits from <see cref="Address"/> and includes additional fields specific to a SalonAddress such as SalonId.
    /// </summary>
    public class SalonAddress : Address
    {
        /// <summary>
        /// Gets the unique identifier of the salon associated with this address.
        /// </summary>
        public Guid SalonId { get; protected set; }
        
        /// <summary>
        /// Gets the salon entity associated with this address.
        /// </summary>
        public Salon? Salon { get; protected set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SalonAddress"/> class.
        /// </summary>
        public SalonAddress(){}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SalonAddress"/> class with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the address.</param>
        /// <param name="salonId">The unique identifier of the salon associated with the address.</param>
        /// <param name="country">The country of the address.</param>
        /// <param name="region">The region of the address.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="postalCode">The postal code of the address.</param>
        /// <param name="street">The street of the address.</param>
        /// <param name="buildingNumber">The building number of the address.</param>
        public SalonAddress(Guid id, Guid salonId, string country, string region, string city, string postalCode,
            string street, string buildingNumber) 
            : base(id, country, region, city, postalCode, street, buildingNumber)
        {
            SetSalonId(salonId);
        }
        
        /// <summary>
        /// Validates and sets the salonId.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to set.</param>
        /// <exception cref="ArgumentException">Thrown when the UserId is empty.</exception>
        private void SetSalonId(Guid salonId)
        {
            SalonId = (salonId != Guid.Empty)
                ? salonId
                : throw new ArgumentException("The salon id cannot be empty", nameof(salonId));
        }
    }
}