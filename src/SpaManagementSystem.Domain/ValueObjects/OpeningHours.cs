namespace SpaManagementSystem.Domain.ValueObjects;

public record OpeningHours
{
    public DayOfWeek DayOfWeek { get; }
    public TimeSpan OpeningTime { get; }
    public TimeSpan ClosingTime { get; }


    
    public OpeningHours(){}
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