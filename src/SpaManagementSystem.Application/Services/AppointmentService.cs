using AutoMapper;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Appointment;

namespace SpaManagementSystem.Application.Services;

public class AppointmentService(
    ISalonRepository salonRepository,
    IEmployeeRepository employeeRepository,
    ICustomerRepository customerRepository,
    IAppointmentRepository appointmentRepository,
    AppointmentBuilder appointmentBuilder,
    AppointmentServiceBuilder appointmentServiceBuilder,
    IMapper mapper) : IAppointmentService
{
    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(request.SalonId));
        var employee =  await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithServicesByIdAsync(request.EmployeeId));
        
        await EnsureEmployeeAvailabilityAsync(request.EmployeeId, request.Date, request.StartTime, request.EndTime);
        
        var customer = await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(request.CustomerId));
        if (customer.IsActive == false)
            throw new InvalidOperationException("Customer is not active.");

        var appointment = appointmentBuilder
            .WithId(Guid.NewGuid())    
            .WithSalonId(request.SalonId)
            .WithEmployeeId(request.EmployeeId)
            .WithCustomerId(request.CustomerId)
            .WithDate(request.Date)
            .WithStartTime(request.StartTime)
            .WithEndTime(request.EndTime)
            .WithNotes(request.Notes)
            .Build();

        foreach (var service in request.Services)
        {
            if (employee.Services.All(es => es.Id != service.ServiceId))
                throw new InvalidOperationException(
                    $"Service with ID {service.ServiceId} is not provided by the selected employee with ID {employee.Id}.");
            
            var appointmentService = appointmentServiceBuilder
                .WithSalonId(request.SalonId)
                .WithAppointmentId(appointment.Id)
                .WithServiceId(service.ServiceId)
                .WithPrice(service.Price)
                .Build();
            
            appointment.AddService(appointmentService);
        }
        
        await appointmentRepository.CreateAsync(appointment);
        await appointmentRepository.SaveChangesAsync();
        
        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(Guid appointmentId)
    {
        var appointment =
            await appointmentRepository.GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));
        
        return mapper.Map<AppointmentDto>(appointment);
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsBySalonIdAsync(Guid salonId,
        DateOnly? startDate, DateOnly? endDate, AppointmentStatus? status)
    {
        await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        var appointments = await appointmentRepository
            .GetAppointmentsBySalonIdAsync(salonId, startDate, endDate, status);

        return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(Guid employeeId,
        DateOnly? startDate, DateOnly? endDate, AppointmentStatus? status)
    {
        await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByIdAsync(employeeId));

        var appointments = await appointmentRepository
            .GetAppointmentsByEmployeeIdAsync(employeeId, startDate, endDate, status);

        return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByCustomerIdAsync(Guid customerId,
        DateOnly? startDate, DateOnly? endDate, AppointmentStatus? status)
    {
        await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(customerId));

        var appointments = await appointmentRepository
            .GetAppointmentsByCustomerIdAsync(customerId, startDate, endDate, status);

        return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
    
    public async Task UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentRequest request)
    {
        var appointment = await appointmentRepository
            .GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));
        
        if (!appointment.CanUpdate)
            throw new InvalidOperationException($"Cannot update appointment when status is {appointment.Status}.");
        
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetWithServicesByIdAsync(request.EmployeeId));
        
        await EnsureEmployeeAvailabilityAsync(request.EmployeeId, request.Date, request.StartTime, request.EndTime, appointmentId);
        
        appointment.UpdateAppointment(request.EmployeeId, request.Date, request.StartTime, request.EndTime, request.Notes);
        
        
        if (request.AddedServices.Any())
            foreach (var service in request.AddedServices)
            {
                if (employee.Services.All(es => es.Id != service.ServiceId))
                    throw new InvalidOperationException(
                        $"Service with ID {service.ServiceId} is not provided by the selected employee with ID {employee.Id}.");
                
                var appointmentService = appointmentServiceBuilder
                    .WithSalonId(appointment.SalonId)
                    .WithAppointmentId(appointment.Id)
                    .WithServiceId(service.ServiceId)
                    .WithPrice(service.Price)
                    .Build();
            
                appointment.AddService(appointmentService);
            }

        if (request.RemovedServices.Any())
            foreach (var service in request.RemovedServices)
                if (appointment.AppointmentServices.Any(x => x.Id == service.AppointmentServiceId))
                    appointment.RemoveService(
                        appointment.AppointmentServices.First(x => x.Id == service.AppointmentServiceId));

        var validationResult = new AppointmentSpecification().IsSatisfiedBy(appointment);

        if (!validationResult.IsValid)
            throw new DomainValidationException($"Appointment update failed: {string.Join(", ", validationResult.Errors)}");
        
        await appointmentRepository.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid appointmentId, AppointmentStatus status)
    {
        var appointment = await appointmentRepository
            .GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));

        appointment.ChangeStatus(status);
        
        await appointmentRepository.SaveChangesAsync();
    }

    public async Task DeleteAppointmentAsync(Guid appointmentId)
    {
        var appointment = await appointmentRepository
            .GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));

        if (!appointment.CanDelete)
            throw new InvalidOperationException("Cannot delete appointment due to its status or because it has associated payments.");

        appointmentRepository.Delete(appointment);
        await appointmentRepository.SaveChangesAsync();
    }
    
    private async Task EnsureEmployeeAvailabilityAsync(Guid employeeId, DateOnly date, DateTime startTime, DateTime endTime,
        Guid? appointmentId = null)
    {
        var isAvailable = await employeeRepository.IsEmployeeAvailableForAppointmentAsync(employeeId, date, startTime, endTime, appointmentId);
        if (!isAvailable)
            throw new InvalidOperationException("Employee is not available at the requested time.");
    }
}