using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Model.Domain;

namespace NzWalk.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this._context = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();

        }
    }
}
