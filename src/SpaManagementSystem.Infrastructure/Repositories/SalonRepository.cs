using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a repository specifically tailored for managing salon entities.
    /// This repository implements both the general repository operations provided through the Repository base class
    /// and the specific operations defined in the ISalonRepository interface.
    /// </summary>
    public class SalonRepository : Repository<Salon>, ISalonRepository
    {
        private readonly SmsDbContext _context;


        
        /// <summary>
        /// Initializes a new instance of the SalonRepository using the specified database context.
        /// This constructor ensures the repository is ready to manage Salon entities by passing the context
        /// to the base repository class.
        /// </summary>
        /// <param name="context">The database context used for Salon entity operations.</param>
        public SalonRepository(SmsDbContext context) : base(context)
        {
            _context = context;
        }


        
        /// <inheritdoc />
        public async Task<Salon?> GetWithDetailsByIdAsync(Guid salonId)
            => await _context.Salons.Include(x => x.OpeningHours).Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == salonId);
        
        /// <inheritdoc />
        public async Task<Salon?> GetByUserIdAsync(Guid userId)
            => await _context.Salons.Include(x => x.OpeningHours).FirstOrDefaultAsync(x => x.UserId == userId);
        
        /// <inheritdoc />
        public async Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId)
            => await Task.FromResult(await _context.Salons.Where(x => x.UserId == userId).ToListAsync());
    }
}