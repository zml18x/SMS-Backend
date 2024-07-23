namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing an address.
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        public string Country { get; set; }
        
        /// <summary>
        /// Gets or sets the region or state of the address.
        /// </summary>
        public string Region { get; set; }
        
        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        public string PostalCode { get; set; }
        
        /// <summary>
        /// Gets or sets the street name of the address.
        /// </summary>
        public string Street { get; set; }
        
        /// <summary>
        /// Gets or sets the building number of the address.
        /// </summary>
        public string BuildingNumber { get; set; }


        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDto"/> class with the specified address details.
        /// </summary>
        /// <param name="country">The country of the address.</param>
        /// <param name="region">The region or state of the address.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="postalCode">The postal code of the address.</param>
        /// <param name="street">The street name of the address.</param>
        /// <param name="buildingNumber">The building number of the address.</param>
        public AddressDto(string country, string region, string city, string postalCode, string street,
            string buildingNumber)
        {
            Country = country;
            Region = region;
            City = city;
            PostalCode = postalCode;
            Street = street;
            BuildingNumber = buildingNumber;
        }
    }
}