using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class AppointmentRepository(SmsDbContext context) : Repository<Appointment>(context), IAppointmentRepository
{
    private readonly SmsDbContext _context = context;

    public new async Task<Appointment?> GetByIdAsync(Guid appointmentId)
        => await _context.Appointments
            .Include(x => x.AppointmentServices)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == appointmentId);
    
    public async Task<(bool salonExists, bool employeeExists, bool customerExists)> ValidateAppointmentEntitiesAsync(
        Guid salonId, Guid employeeId, Guid customerId)
    {
        var salonExists = await _context.Salons.AnyAsync(s => s.Id == salonId);
        
        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == employeeId && e.SalonId == salonId && e.EmploymentStatus == EmploymentStatus.Active);

        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == customerId && c.SalonId == salonId && c.IsActive == true);
        
        return (salonExists, employeeExists, customerExists);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsBySalonIdAsync(Guid salonId, DateOnly? startDate = null,
        DateOnly? endDate = null, AppointmentStatus? status = null)
        => await GetAppointmentsAsync(salonId: salonId, startDate: startDate, endDate: endDate, status: status);
    
    public async Task<IEnumerable<Appointment>> GetAppointmentsByEmployeeIdAsync(Guid employeeId, DateOnly? startDate = null,
        DateOnly? endDate = null, AppointmentStatus? status = null)
        => await GetAppointmentsAsync(employeeId: employeeId, startDate: startDate, endDate: endDate, status: status);
    
    public async Task<IEnumerable<Appointment>> GetAppointmentsByCustomerIdAsync(Guid customerId, DateOnly? startDate = null,
        DateOnly? endDate = null, AppointmentStatus? status = null)
        => await GetAppointmentsAsync(customerId: customerId, startDate: startDate, endDate: endDate, status: status);
    
    private async Task<IEnumerable<Appointment>> GetAppointmentsAsync(Guid? salonId = null,
        Guid? employeeId = null, Guid? customerId = null, DateOnly? startDate = null, DateOnly? endDate = null,
        AppointmentStatus? status = null)
    {
        var query = _context.Appointments
            .Include(x => x.AppointmentServices)
            .Include(x => x.Payments)
            .AsQueryable();
        
        if (salonId.HasValue)
            query = query.Where(x => x.SalonId == salonId);
        
        if (employeeId.HasValue)
            query = query.Where(x => x.EmployeeId == employeeId);
        
        if (customerId.HasValue)
            query = query.Where(x => x.CustomerId == customerId);
        
        if (status.HasValue)
            query = query.Where(x => x.Status == status);
        
        if (startDate.HasValue)
            query = query.Where(x => x.Date >= startDate);
        
        if (endDate.HasValue)
            query = query.Where(x => x.Date <= endDate);
        
        return await query.ToListAsync();
    }
}