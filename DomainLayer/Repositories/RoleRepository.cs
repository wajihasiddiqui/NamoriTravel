using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public RoleRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<Role> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Roles
                .Where(x => x.RoleName.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
}
