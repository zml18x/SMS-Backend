using FluentAssertions;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.UnitTests.SpecificationTests;

public class EmployeeSpecificationTests
{
    private Employee CreateEmployee(
        Guid? employeeId = null,
        Guid? salonId = null,
        Guid? userId = null,
        string position = "Example Position",
        EmploymentStatus employmentStatus = EmploymentStatus.Active,
        string code = "Example Code",
        DateOnly? hireDate = null,
        string color = "#ffffff",
        string notes = "Example Notes")
    {
        return new Employee(
            employeeId ?? Guid.NewGuid(),
            salonId ?? Guid.NewGuid(),
            userId ?? Guid.NewGuid(),
            position,
            employmentStatus,
            code,
            hireDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            color,
            notes
        );
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnNoErrors_When_AllDataIsValid()
    {
        // Arrange
        var employee = CreateEmployee();
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_SalonIdIsEmpty()
    {
        // Arrange
        var employee = CreateEmployee(salonId: Guid.Empty);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("SalonId is required (Cannot be Guid.Empty).");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_UserIdIsEmpty()
    {
        // Arrange
        var employee = CreateEmployee(userId: Guid.Empty);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("UserId is required (Cannot be Guid.Empty).");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_PositionIsNullOrEmpty()
    {
        // Arrange
        var employee = CreateEmployee(position: null!);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Position is required.");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_EmploymentStatusIsNotDefinedEnum()
    {
        // Arrange
        var invalidEmploymentStatus = (EmploymentStatus)4;
        var employee = CreateEmployee(employmentStatus: invalidEmploymentStatus);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain($"Invalid employment status: {invalidEmploymentStatus}");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_CodeIsNullOrEmpty()
    {
        // Arrange
        var employee = CreateEmployee(code: null!);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Code is required.");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_HireDateIsInTheFuture()
    {
        // Arrange
        var employee = CreateEmployee(hireDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain($"Hire date {employee.HireDate} cannot be in the future.");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_HireDateIsTooFarInThePast()
    {
        // Arrange
        var employee = CreateEmployee(hireDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-51)));
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain($"Hire date {employee.HireDate} is too far in the past.");
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnErrors_When_NotesAreLongerThan500Characters()
    {
        // Arrange
        var invalidNotes = new string('a', 501);
        var employee = CreateEmployee(notes: invalidNotes);
        var specification = new EmployeeSpecification();

        // Act
        var result = specification.IsSatisfiedBy(employee);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Notes cannot be longer than 500 characters.");
    }
}