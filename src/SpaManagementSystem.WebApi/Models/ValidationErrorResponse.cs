namespace SpaManagementSystem.WebApi.Models;

public class ValidationErrorResponse()
{
    public int Status { get; set; } = StatusCodes.Status400BadRequest;
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "Validation errors occurred.";
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
