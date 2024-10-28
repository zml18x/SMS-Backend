namespace SpaManagementSystem.Domain.Interfaces;

public interface IUniqueCodeRepository
{
    public Task<bool> IsExistsAsync(Guid salonId, string code);
}
