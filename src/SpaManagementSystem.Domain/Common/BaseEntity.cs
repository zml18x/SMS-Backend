namespace SpaManagementSystem.Domain.Common
{
    /// <summary>
    /// Serves as the base class for all entity models in the Spa Management System.
    /// This class provides common properties and initialization logic to ensure consistency across all entities.
    /// It includes an ID, and timestamps for creation and last update to track entity lifecycle events.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets the unique identifier for the entity. This ID should be provided during instantiation.
        /// </summary>
        public Guid Id { get; protected set; }
        
        /// <summary>
        /// Gets the date and time when the entity was created.
        /// This is set to the current UTC time when the entity is instantiated.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }
        
        /// <summary>
        /// Gets the date and time when the entity was last updated.
        /// Initially set to the current UTC time at instantiation,
        /// and should be updated whenever changes are made to the entity.
        /// </summary>
        public DateTime UpdatedAt { get; protected set; }
        
        
        
        /// <summary>
        /// Initializes a new instance of the BaseEntity class.
        /// This parameterless constructor is protected to prevent direct instantiation
        /// and ensure that derived classes control the creation process.
        /// </summary>
        protected BaseEntity(){}
        
        /// <summary>
        /// Initializes a new instance of the BaseEntity class with a specified identifier.
        /// This constructor sets the id to the provided GUID and initializes the CreatedAt and UpdatedAt properties
        /// to the current UTC date and time, ensuring that each entity has a distinct and traceable lifecycle start point.
        /// </summary>
        /// <param name="id">The unique identifier for the entity, to be provided upon creation.</param>
        protected BaseEntity(Guid id)
        {
            SetId(id);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }



        /// <summary>
        /// Validates and sets the id.
        /// </summary>
        /// <param name="id">The unique identifier to set.</param>
        /// <exception cref="ArgumentException">Thrown when the id is empty.</exception>
        private void SetId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(id));

            Id = id;
        }
    }
}