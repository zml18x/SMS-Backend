namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a salon.
    /// </summary>
    public class SalonDto
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
        /// Initializes a new instance of the <see cref="SalonDto"/> class with the specified identifier and name.
        /// </summary>
        /// <param name="id">The unique identifier of the salon.</param>
        /// <param name="name">The name of the salon.</param>
        public SalonDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}