namespace SpaManagementSystem.Domain.Specifications;

public interface ISpecification<in T> where T : class
{
    public ValidationResult IsSatisfiedBy(T entity);
}