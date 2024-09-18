namespace SpaManagementSystem.Domain.Specifications;

public class ValidationResult(bool isValid)
{
    private readonly List<string> _errors = new();
    public bool IsValid { get; protected set; } = isValid;
    public IEnumerable<string> Errors => _errors;
        
    
    
    public void AddError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
        {
            _errors.Add(error);
            IsValid = false;
        }
    }
}