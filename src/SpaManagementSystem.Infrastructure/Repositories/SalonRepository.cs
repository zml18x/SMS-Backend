using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories
{
    public class SalonRepository : Repository<Salon>, ISalonRepository
    {
        private readonly SmsDbContext _context;



        public SalonRepository(SmsDbContext context) : base(context)
        {
            _context = context;
        }



        public new async Task<Salon?> GetByIdAsync(Guid salonId)
            => await _context.Salons.Include(x => x.OpeningHours).FirstOrDefaultAsync(x => x.Id == salonId);

        public async Task<Salon?> GetByUserIdAsync(Guid userId)
            => await _context.Salons.Include(x => x.OpeningHours).FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId)
            => await Task.FromResult(await _context.Salons.Where(x => x.UserId == userId).ToListAsync());
    }
}