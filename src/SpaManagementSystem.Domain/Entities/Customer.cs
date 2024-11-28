using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Customer : BaseEntity
{
    private ISet<Appointment> _appointments = new HashSet<Appointment>();
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }  = null!;
    
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public GenderType Gender {get; protected set; }
    public string PhoneNumber { get; protected set; }
    public string? Email { get; protected set; }
    public IEnumerable<Appointment> Appointments => _appointments;


    
    protected Customer(){}
    public Customer(Guid id, Guid salonId, string firstName, string lastName, GenderType gender, string phoneNumber,
        string? email = null)
    {
        Id = id;
        SalonId = salonId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}