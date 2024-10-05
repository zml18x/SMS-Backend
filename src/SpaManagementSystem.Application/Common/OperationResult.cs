namespace SpaManagementSystem.Application.Common;

public class OperationResult
{
    public bool IsSuccess { get; private set; }
    public bool HasValidationErrors => Errors.Count == 0;
    public Dictionary<string, string[]> Errors { get; private init; } = new();

    
    
    public static OperationResult Success() => new() { IsSuccess = true };
    public static OperationResult NoChanges() => new () { IsSuccess = true };
    public static OperationResult ValidationFailed(Dictionary<string, string[]> errors)
        => new() { IsSuccess = false, Errors = errors };
}