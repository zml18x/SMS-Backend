namespace SpaManagementSystem.Application.Dto
{
    public class SalonDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public IEnumerable<OpeningHoursDto> OpeningHours { get; set; }



        public SalonDetailsDto(Guid id, string name, string email, string phoneNumber, string? description,
            IEnumerable<OpeningHoursDto> openingHours)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
            OpeningHours = openingHours;
        }
    }
}