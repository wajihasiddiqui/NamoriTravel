using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
  
    public class RateBasisRepository : GenericRepository<RateBasis>, IRateBasisRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public RateBasisRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<RateBasis> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.RateBases
                .Where(x => (x.Description.Contains(SerachValue))
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }

    public class BusinessRepository : GenericRepository<Busines>, IBusinessRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public BusinessRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Busines> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Businesses
               .Where(x => (x.Description.Contains(SerachValue))
               && x.IsActive
               && !x.IsDeleted)
               .ToListAsync();
            return (result, result.Count());
        }
    }

    public class AmenitiesRepository : GenericRepository<Amenities>, IAmenitiesRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public AmenitiesRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Amenities> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Amenities
               .Where(x => (x.Description.Contains(SerachValue))
               && x.IsActive
               && !x.IsDeleted)
               .ToListAsync();
            return (result, result.Count());
        }
    }
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public CurrencyRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Currency> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Currency
                .Where(x => (x.Description.Contains(SerachValue) || x.Shortcut == SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
}
