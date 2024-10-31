using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class DotwRequestRepository : GenericRepository<DotwRequest>, IDotwRequestRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public DotwRequestRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<DotwRequest> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.DotwRequests
                .Where(x => x.Username.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
}
