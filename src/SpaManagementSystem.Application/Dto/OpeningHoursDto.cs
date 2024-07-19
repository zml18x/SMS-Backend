namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing the opening hours of a salon for a specific day.
    /// </summary>
    public class OpeningHoursDto
    {
        /// <summary>
        /// Gets or sets the day of the week for which the opening hours are specified.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the opening time of the salon on the specified day.
        /// </summary>
        public TimeSpan OpeningTime { get; set; }

        /// <summary>
        /// Gets or sets the closing time of the salon on the specified day.
        /// </summary>
        public TimeSpan ClosingTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the salon is closed on the specified day.
        /// </summary>
        public bool IsClosed { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningHoursDto"/> class with the specified details.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week for which the opening hours are specified.</param>
        /// <param name="openingTime">The opening time of the salon on the specified day.</param>
        /// <param name="closingTime">The closing time of the salon on the specified day.</param>
        /// <param name="isClosed">A boolean value indicating whether the salon is closed on the specified day.</param>
        public OpeningHoursDto(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime, bool isClosed)
        {
            DayOfWeek = dayOfWeek;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            IsClosed = isClosed;
        }
    }
}