using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities
{
    /// <summary>
    /// Represents an abstract base class for an address entity.
    /// </summary>
    public abstract class Address : BaseEntity
    {
        /// <summary>
        /// Gets the country of the address.
        /// </summary>
        public string Country { get; protected set; } = string.Empty;
        
        /// <summary>
        /// Gets the region of the address.
        /// </summary>
        public string Region { get; protected set; } = string.Empty;
        
        /// <summary>
        /// Gets the city of the address.
        /// </summary>
        public string City { get; protected set; } = string.Empty;
        
        /// <summary>
        /// Gets the postal code of the address.
        /// </summary>
        public string PostalCode { get; protected set; } = string.Empty;
        
        /// <summary>
        /// Gets the street of the address.
        /// </summary>
        public string Street { get; protected set; } = string.Empty;
        
        /// <summary>
        /// Gets the building number of the address.
        /// </summary>
        public string BuildingNumber { get; protected set; } = string.Empty;
        
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        protected Address() {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the address.</param>
        /// <param name="country">The country of the address.</param>
        /// <param name="region">The region of the address.</param>
        /// <param name="city">The city of the address.</param>
        /// <param name="postalCode">The postal code of the address.</param>
        /// <param name="street">The street of the address.</param>
        /// <param name="buildingNumber">The building number of the address.</param>
        protected Address(Guid id, string country, string region, string city,
            string postalCode, string street, string buildingNumber) : base(id)
        {
            SetCountry(country);
            SetRegion(region);
            SetCity(city);
            SetPostalCode(postalCode);
            SetStreet(street);
            SetBuildingNumber(buildingNumber);
        }


        
        /// <summary>
        /// Updates the address properties.
        /// </summary>
        /// <param name="country">The new country value.</param>
        /// <param name="region">The new region value.</param>
        /// <param name="city">The new city value.</param>
        /// <param name="postalCode">The new postal code value.</param>
        /// <param name="street">The new street value.</param>
        /// <param name="buildingNumber">The new building number value.</param>
        /// <returns>True if any data was updated; otherwise, false.
        /// This can be used to determine if the entity needs to be saved to the database.</returns>
        public bool UpdateAddress(string country, string region, string city, string postalCode, string street,
            string buildingNumber)
        {
            var anyDataUpdated = false;

            if (!string.IsNullOrWhiteSpace(country))
            {
                SetCountry(country);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                SetRegion(region);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                SetCity(city);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                SetPostalCode(postalCode);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(street))
            {
                SetStreet(street);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(buildingNumber))
            {
                SetBuildingNumber(buildingNumber);
                anyDataUpdated = true;
            }

            return anyDataUpdated;
        }
        
        /// <summary>
        /// Validates and sets the country.
        /// </summary>
        /// <param name="country">The country to set.</param>
        /// <exception cref="ArgumentException">Thrown when the country is null or whitespace.</exception>
        private void SetCountry(string country)
        {
            Country = !string.IsNullOrWhiteSpace(country)
                ? country
                : throw new ArgumentException("The country of address cannot be empty or whitespace.", nameof(country));
        }
        
        /// <summary>
        /// Validates and sets the region.
        /// </summary>
        /// <param name="region">The region to set.</param>
        /// <exception cref="ArgumentException">Thrown when the region is null or whitespace.</exception>
        private void SetRegion(string region)
        {
            Region = !string.IsNullOrWhiteSpace(region)
                ? region
                : throw new ArgumentException("The region of address cannot be empty or whitespace.", nameof(region));
        }
        
        /// <summary>
        /// Validates and sets the city.
        /// </summary>
        /// <param name="city">The city to set.</param>
        /// <exception cref="ArgumentException">Thrown when the city is null or whitespace.</exception>
        private void SetCity(string city)
        {
            City = !string.IsNullOrWhiteSpace(city)
                ? city
                : throw new ArgumentException("The city of address cannot be empty or whitespace.", nameof(city));
        }
        
        /// <summary>
        /// Validates and sets the postal code.
        /// </summary>
        /// <param name="postalCode">The postal code to set.</param>
        /// <exception cref="ArgumentException">Thrown when the postal code is null or whitespace.</exception>
        private void SetPostalCode(string postalCode)
        {
            PostalCode = !string.IsNullOrWhiteSpace(postalCode)
                ? postalCode
                : throw new ArgumentException("The postal code of address cannot be empty or whitespace.",
                    nameof(postalCode));
        }
        
        /// <summary>
        /// Validates and sets the street.
        /// </summary>
        /// <param name="street">The street to set.</param>
        /// <exception cref="ArgumentException">Thrown when the street is null or whitespace.</exception>
        private void SetStreet(string street)
        {
            Street = !string.IsNullOrWhiteSpace(street)
                ? street
                : throw new ArgumentException("The street of address cannot be empty or whitespace.", nameof(street));
        }
        
        /// <summary>
        /// Validates and sets the buildingNumber.
        /// </summary>
        /// <param name="buildingNumber">The buildingNumber to set.</param>
        /// <exception cref="ArgumentException">Thrown when the buildingNumber is null or whitespace.</exception>
        private void SetBuildingNumber(string buildingNumber)
        {
            BuildingNumber = !string.IsNullOrWhiteSpace(buildingNumber)
                ? buildingNumber
                : throw new ArgumentException("The building number of address cannot be empty or whitespace.",
                    nameof(buildingNumber));
        }
    }
}