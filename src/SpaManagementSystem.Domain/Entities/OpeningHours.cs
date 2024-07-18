using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities
{
    public class OpeningHours : BaseEntity
    {
        public Guid SalonId { get; protected set; }
        public DayOfWeek DayOfWeek { get; protected set; }
        public TimeSpan OpeningTime { get; protected set; }
        public TimeSpan ClosingTime { get; protected set; }
        public bool IsClosed { get; protected set; }
        public Salon Salon { get; set; }


    
        public OpeningHours(){}
        public OpeningHours(Guid id, Guid salonId, DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime,
            bool isClosed = false) : base(id)
        {
            SetSalonId(salonId);
            SetDayOfWeek(dayOfWeek);
            SetOpeningHours(openingTime, closingTime);
            IsClosed = isClosed;
        }



        public void UpdateHours(TimeSpan openingTime, TimeSpan closingTime, bool isClosed)
        {
            SetOpeningHours(openingTime, closingTime);
            IsClosed = isClosed;
        }
        
        private void SetSalonId(Guid salonId)
        {
            SalonId = (salonId != Guid.Empty)
                ? salonId
                : throw new ArgumentException("The salon id cannot be empty", nameof(salonId));
        }
        
        private void SetDayOfWeek(DayOfWeek dayOfWeek)
        {
            DayOfWeek = (Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
                ? dayOfWeek
                : throw new ArgumentException("Invalid value for the DayOfWeek.", nameof(dayOfWeek));
        }

        private void SetOpeningHours(TimeSpan openingTime, TimeSpan closingTime)
        {
            if (closingTime <= openingTime)
                throw new ArgumentException("Closing time must be after opening time", nameof(closingTime));

            OpeningTime = openingTime;
            ClosingTime = closingTime;
        }
    }
}