using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class SalonBuilder(ISpecification<Salon> specification) : IBuilder<Salon>
{
#nullable disable
    private Guid _id = Guid.Empty;
    private Guid _userId;
    private string _name;
    private string _email;
    private string _phoneNumber;
#nullable enable
    private string? _description;


    public Salon Build()
    {
        var salon = new Salon(_id, _userId, _name, _email, _phoneNumber, _description);

        var validationResult = specification.IsSatisfiedBy(salon);

        if (!validationResult.IsValid)
            throw new DomainValidationException($"Salon creation failed: {string.Join(", ", validationResult.Errors)}");

        return salon;
    }
    
    public SalonBuilder WithSalonId(Guid salonId)
    {
        _id = salonId;
        return this;
    }
    
    public SalonBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public SalonBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public SalonBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }
    
    public SalonBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }
    
    public SalonBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
}