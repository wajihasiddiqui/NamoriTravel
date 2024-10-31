using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;
using System.Buffers;

namespace DomainLayer.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public CountryRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Country> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Country
                .Where(x => (x.Name.Contains(SerachValue)
                || x.Code.Contains(SerachValue))
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public CityRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<City> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.City
                .Where(x => (x.Name.Contains(SerachValue)
                || x.Code.Contains(SerachValue) 
                || x.CountryCode.Contains(SerachValue)
                )
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }

        public async Task<IEnumerable<City>> GetAllByCountryCodeAsync(string? Code)
        {
            var result = await _context.City
                .Where(x => x.CountryCode == Code
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return result;
        }

        
    }
}
