using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class CustomerRepository(SmsDbContext context) : Repository<Customer>(context), ICustomerRepository
{
    private readonly SmsDbContext _context = context;
}