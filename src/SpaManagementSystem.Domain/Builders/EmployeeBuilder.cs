using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class EmployeeBuilder(ISpecification<Employee> employeeSpecification,
    ISpecification<EmployeeProfile> employeeProfileSpecification) : IBuilder<Employee>
{
    // Employee
#nullable disable
    private Guid _id = Guid.Empty;
    private Guid _salonId;
    private Guid _userId;
    private string _position;
    private EmploymentStatus _employmentStatus;
    private string _code;
    private string _color;
    private DateOnly _hireDate;
#nullable enable
    private string? _notes;
    
#nullable disable
    // EmployeeProfile
    private string _firstName;
    private string _lastName;
    private GenderType _gender;
    private DateOnly _dateOfBirth;
    private string _email;
    private string _phoneNumber;
#nullable enable
    
    
    
    public Employee Build()
    {
        var employee = new Employee(_id, _salonId, _userId, _position, _employmentStatus,
            _code, _hireDate, _color, _notes);
        
        var validationResult = employeeSpecification.IsSatisfiedBy(employee);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Employee creation failed: {string.Join(", ", validationResult.Errors)}");
        
        return employee;
    }

    public EmployeeProfile BuildEmployeeProfile()
    {
        var employeeProfile = new EmployeeProfile(_firstName, _lastName, _gender, _dateOfBirth, _email, _phoneNumber);

        var validationResult = employeeProfileSpecification.IsSatisfiedBy(employeeProfile);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Employee profile creation failed: {string.Join(", ", validationResult.Errors)}");
        
        return employeeProfile;
    }

    public EmployeeBuilder WithId(Guid employeeId)
    {
        _id = employeeId;
        return this;
    }
    
    public EmployeeBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }
    
    public EmployeeBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }
    
    public EmployeeBuilder WithPosition(string position)
    {
        _position = position;
        return this;
    }
    
    public EmployeeBuilder WithEmploymentStatus(EmploymentStatus employmentStatus)
    {
        _employmentStatus = employmentStatus;
        return this;
    }
    
    public EmployeeBuilder WithCode(string code)
    {
        _code = code.ToUpper();
        return this;
    }
    
    public EmployeeBuilder WithColor(string color)
    {
        _color = color;
        return this;
    }
    
    public EmployeeBuilder WithHireDate(DateOnly hireDate)
    {
        _hireDate = hireDate;
        return this;
    }

    public EmployeeBuilder WithNotes(string notes)
    {
        _notes = notes;
        return this;
    }
    
    // EmployeeProfile
    public EmployeeBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }
    
    public EmployeeBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }
    
    public EmployeeBuilder WithGender(GenderType gender)
    {
        _gender = gender;
        return this;
    }
    
    public EmployeeBuilder WithDateOfBirth(DateOnly dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }
    
    public EmployeeBuilder WithEmail(string email)
    {
        _email = email.ToLower();
        return this;
    }

    public EmployeeBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }
}