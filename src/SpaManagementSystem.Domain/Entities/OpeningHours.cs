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
            Id = id;
            SalonId = salonId;
            DayOfWeek = dayOfWeek;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            IsClosed = isClosed;
        }



        public void UpdateHours(TimeSpan openingTime, TimeSpan closingTime, bool isClosed)
        {
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            IsClosed = isClosed;
        }
    }
}