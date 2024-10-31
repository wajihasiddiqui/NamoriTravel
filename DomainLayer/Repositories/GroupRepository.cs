using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class GroupRepository : GenericRepository<Groups>, IGroupRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public GroupRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Groups> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Groups
                .Where(x => x.GroupName.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
}
