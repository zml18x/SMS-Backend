namespace SpaManagementSystem.Domain.ValueObjects;

public record AvailableHours
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    
    
    
    public AvailableHours(TimeSpan start, TimeSpan end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be earlier than end time.");
        
        Start = start;
        End = end;
    }

    
    
    public bool Overlaps(AvailableHours other)
        => Start < other.End && End > other.Start;
}