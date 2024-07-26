using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

/// <summary>
///Represents the salon opening hours for a specific day within the Spa Management System.
/// Inherits from BaseEntity and includes fields for the salon ID, day of the week, opening and closing times,
/// and whether the salon is closed on that day.
/// </summary>
public class OpeningHours : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for the associated salon.
    /// </summary>
    public Guid SalonId { get; protected set; }

    /// <summary>
    /// Gets the day of the week for the opening hours.
    /// </summary>
    public DayOfWeek DayOfWeek { get; protected set; }

    /// <summary>
    /// Gets the opening time for the day.
    /// </summary>
    public TimeSpan OpeningTime { get; protected set; }

    /// <summary>
    /// Gets the closing time for the day.
    /// </summary>
    public TimeSpan ClosingTime { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the salon is closed on this day.
    /// </summary>
    public bool IsClosed { get; protected set; }

    /// <summary>
    /// Gets or sets the associated salon.
    /// </summary>
    public Salon Salon { get; set; }

    /// <summary>
    /// Initializes a new instance of the OpeningHours class.
    /// </summary>
    public OpeningHours() {}
        
    /// <summary>
    /// Initializes a new instance of the OpeningHours class with specific details.
    /// </summary>
    /// <param name="id">The unique identifier for the opening hours.</param>
    /// <param name="salonId">The unique identifier for the associated salon.</param>
    /// <param name="dayOfWeek">The day of the week for the opening hours.</param>
    /// <param name="openingTime">The opening time for the day.</param>
    /// <param name="closingTime">The closing time for the day.</param>
    /// <param name="isClosed">A value indicating whether the salon is closed on this day.</param>
    public OpeningHours(Guid id, Guid salonId, DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime,
        bool isClosed = false) : base(id)
    {
        SetSalonId(salonId);
        SetDayOfWeek(dayOfWeek);
        SetOpeningHours(openingTime, closingTime);
        IsClosed = isClosed;
    }


        
    /// <summary>
    /// Updates the opening hours for the day.
    /// </summary>
    /// <param name="openingTime">The new opening time to set.</param>
    /// <param name="closingTime">The new closing time to set.</param>
    /// <param name="isClosed">A value indicating whether the salon is closed on this day.</param>
    public void UpdateHours(TimeSpan openingTime, TimeSpan closingTime, bool isClosed)
    {
        SetOpeningHours(openingTime, closingTime);
        IsClosed = isClosed;
    }
        
    /// <summary>
    /// Validates and sets the SalonId.
    /// </summary>
    /// <param name="salonId">The unique identifier for the associated salon.</param>
    /// <exception cref="ArgumentException">Thrown when the salonId is empty.</exception>
    private void SetSalonId(Guid salonId)
    {
        SalonId = (salonId != Guid.Empty)
            ? salonId
            : throw new ArgumentException("The salon id cannot be empty", nameof(salonId));
    }
        
    /// <summary>
    /// Validates and sets the DayOfWeek.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week to set.</param>
    /// <exception cref="ArgumentException">Thrown when the dayOfWeek is not a defined enum value.</exception>
    private void SetDayOfWeek(DayOfWeek dayOfWeek)
    {
        DayOfWeek = (Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
            ? dayOfWeek
            : throw new ArgumentException("Invalid value for the DayOfWeek.", nameof(dayOfWeek));
    }
        
    /// <summary>
    /// Validates and sets the opening and closing times.
    /// </summary>
    /// <param name="openingTime">The opening time to set.</param>
    /// <param name="closingTime">The closing time to set.</param>
    /// <exception cref="ArgumentException">Thrown when the closing time is not after the opening time.</exception>
    private void SetOpeningHours(TimeSpan openingTime, TimeSpan closingTime)
    {
        if (closingTime <= openingTime)
            throw new ArgumentException("Closing time must be after opening time", nameof(closingTime));

        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }
}