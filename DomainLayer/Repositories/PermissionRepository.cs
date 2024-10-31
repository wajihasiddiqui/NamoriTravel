using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public PermissionRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Permission> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Permissions
                .Where(x => x.PermissionName.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }

    }
}
