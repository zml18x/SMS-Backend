using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    public Task<IEnumerable<Payment>> GetPaymentsForCustomerAsync(Guid customerId, DateOnly? startDate = null,
        DateOnly? endDate = null);
}