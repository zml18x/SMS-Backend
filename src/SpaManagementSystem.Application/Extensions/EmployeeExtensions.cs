using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Service;

namespace SpaManagementSystem.Application.Extensions;

public static class EmployeeExtensions
{
    public static bool HasChanges(this Employee existingEmployee, UpdateEmployeeSelfRequest request)
    {
        return existingEmployee.Color != request.Color ||
               existingEmployee.Notes != request.Notes;
    }
    
    public static bool HasChanges(this Employee existingEmployee, UpdateEmployeeRequest request)
    {
        return existingEmployee.Position != request.Position ||
               existingEmployee.EmploymentStatus != request.EmploymentStatus ||
               existingEmployee.Code.ToUpper() != request.Code.ToUpper() ||
               existingEmployee.Color != request.Color ||
               existingEmployee.HireDate != request.HireDate ||
               existingEmployee.Notes != request.Notes;
    }
    
    public static bool HasChanges(this EmployeeProfile existingEmployeeProfile, UpdateEmployeeProfileSelfRequest request)
    {
        return existingEmployeeProfile.Email != request.Email ||
               existingEmployeeProfile.PhoneNumber != request.PhoneNumber;
    }
    
    public static bool HasChanges(this EmployeeProfile existingEmployeeProfile, UpdateEmployeeProfileRequest request)
    {
        return existingEmployeeProfile.FirstName != request.FirstName ||
               existingEmployeeProfile.LastName != request.LastName ||
               existingEmployeeProfile.Gender != request.Gender ||
               existingEmployeeProfile.DateOfBirth != request.DateOfBirth ||
               existingEmployeeProfile.Email != request.Email ||
               existingEmployeeProfile.PhoneNumber != request.PhoneNumber;
    }
    
    public static bool HasChanges(this Service existingService, UpdateServiceRequest request)
    {
        return existingService.Name != request.Name ||
               existingService.Code.ToUpper() != request.Code.ToUpper() ||
               existingService.Description != request.Description ||
               existingService.Price != request.Price ||
               existingService.TaxRate != request.TaxRate ||
               existingService.Duration != request.Duration ||
               existingService.ImgUrl != request.ImgUrl ||
               existingService.IsActive != request.IsActive;
    }
}