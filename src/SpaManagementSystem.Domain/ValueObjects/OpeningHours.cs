namespace SpaManagementSystem.Domain.ValueObjects;

/// <summary>
/// Represents the opening hours for a specific day of the week.
/// This class encapsulates the information about the opening and closing times for a given day.
/// </summary>
public record OpeningHours
{
    public DayOfWeek DayOfWeek { get; }
    public TimeSpan OpeningTime { get; }
    public TimeSpan ClosingTime { get; }


    
    public OpeningHours(){}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="OpeningHours"/> class with specified values.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week for which the opening hours apply.</param>
    /// <param name="openingTime">The opening time on the specified day.</param>
    /// <param name="closingTime">The closing time on the specified day.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="dayOfWeek"/> is not a valid <see cref="DayOfWeek"/>
    /// value or when <paramref name="closingTime"/> is not after <paramref name="openingTime"/>.</exception>
    public OpeningHours(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime)
    {
        DayOfWeek = (Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
            ? dayOfWeek
            : throw new ArgumentException(
                "Invalid value for DayOfWeek. It must be an integer between 0 and 6, representing days of the week (Sunday to Saturday).",
                nameof(dayOfWeek));

        if (closingTime <= openingTime)
            throw new ArgumentException("Closing time must be after opening time", nameof(closingTime));

        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }
}