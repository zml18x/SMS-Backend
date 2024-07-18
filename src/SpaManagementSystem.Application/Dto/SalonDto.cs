namespace SpaManagementSystem.Application.Dto
{
    public class SalonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        
        
        public SalonDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}