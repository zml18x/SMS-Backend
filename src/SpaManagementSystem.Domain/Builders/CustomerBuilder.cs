using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class CustomerBuilder(ISpecification<Customer> specification) : IBuilder<Customer>
{
    private Guid _id = Guid.Empty;
    private Guid _salonId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private GenderType _gender = GenderType.Male;
    private string _phoneNumber = string.Empty;
    private string? _email;
    private string? _notes;
    
    public Customer Build()
    {
        var customer = new Customer(_id, _salonId, _firstName, _lastName, _gender, _phoneNumber, _email, _notes);
        
        var validationResult = specification.IsSatisfiedBy(customer);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Customer creation failed: {string.Join(", ", validationResult.Errors)}");

        return customer;
    }

    public CustomerBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public CustomerBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }
    
    public CustomerBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }
    
    public CustomerBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }
    
    public CustomerBuilder WithGender(GenderType gender)
    {
        _gender = gender;
        return this;
    }
    
    public CustomerBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }
    
    public CustomerBuilder WithEmail(string? email)
    {
        
        _email = email?.ToLower();
        return this;
    }

    public CustomerBuilder WithNotes(string? notes)
    {
        _notes = notes;
        return this;
    }
}