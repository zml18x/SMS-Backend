using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Entities;

public class Salon : BaseEntity
{
    private ISet<OpeningHours> _openingHours = new HashSet<OpeningHours>();
    private ISet<Employee> _employees = new HashSet<Employee>();
    private ISet<Product> _products = new HashSet<Product>();
    private ISet<Service> _services = new HashSet<Service>();
    private ISet<Customer> _customers = new HashSet<Customer>();
    private ISet<Appointment> _appointments = new HashSet<Appointment>();
    public Guid UserId { get; protected set; }
    public string Name { get; protected set; } = String.Empty;
    public string Email { get; protected set; } = String.Empty;
    public string PhoneNumber { get; protected set; } = String.Empty;
    public string? Description { get; protected set; }
    public Address? Address { get; protected set; }
    public IEnumerable<OpeningHours> OpeningHours => _openingHours;
    public IEnumerable<Employee> Employees => _employees;
    public IEnumerable<Product> Products => _products;
    public IEnumerable<Service> Services => _services;
    public IEnumerable<Customer> Customers => _customers;
    public IEnumerable<Appointment> Appointments => _appointments;
        

    
    protected Salon(){}
    public Salon(Guid id, Guid userId, string name, string email, string phoneNumber, string? description)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Description = description;
    }


    
    public bool UpdateSalon(string name, string email, string phoneNumber, string? description)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Name = name, Name != name },
            { () => Email = email, Email != email },
            { () => PhoneNumber = phoneNumber, PhoneNumber != phoneNumber },
            { () => Description = description, Description != description }
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
    
    public void SetAddress(Address address)
    {
        Address = address;
        UpdateTimestamp();
    }
    
    public void AddOpeningHours(OpeningHours openingHours)
    {
        if (_openingHours.Any(x => x.DayOfWeek == openingHours.DayOfWeek))
            throw new InvalidOperationException($"Opening hours for {openingHours.DayOfWeek} already exist.");

        _openingHours.Add(openingHours);
        UpdateTimestamp();
    }
    
    public void UpdateOpeningHours(OpeningHours openingHours)
    {
        var existingOpeningHours = _openingHours.FirstOrDefault(oh => oh.DayOfWeek == openingHours.DayOfWeek);
        if (existingOpeningHours == null)
            throw new InvalidOperationException($"No opening hours found for {openingHours.DayOfWeek}.");

        _openingHours.Remove(existingOpeningHours);
        _openingHours.Add(openingHours);
        UpdateTimestamp();
    }
    
    public void RemoveOpeningHours(DayOfWeek dayOfWeek)
    {
        var openingHours = _openingHours.FirstOrDefault(oh => oh.DayOfWeek == dayOfWeek);

        if (openingHours == null)
            throw new InvalidOperationException($"No opening hours found for {dayOfWeek}.");

        _openingHours.Remove(openingHours);
        UpdateTimestamp();
    }
    
    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }
    
    public void AddService(Service service)
    {
        _services.Add(service);
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public void AddCustomer(Customer customer)
    {
        _customers.Add(customer);
    }
}