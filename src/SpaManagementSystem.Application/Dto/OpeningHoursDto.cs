namespace SpaManagementSystem.Application.Dto
{
    public class OpeningHoursDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsClosed { get; set; }



        public OpeningHoursDto(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime, bool isClosed)
        {
            DayOfWeek = dayOfWeek;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            IsClosed = isClosed;
        }
    }
}