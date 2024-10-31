using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        private readonly DbSet<Page> _dbSet;
        public PageRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Page>();
        }
        public async Task<IEnumerable<PagePermissionsObj>> GetPagePermissionsByGroupId(int groupId)
        {
            var pagePermissions = await (from pp in _context.PagePermissions
                                         join p in _context.Pages on pp.PageId equals p.Id
                                         where pp.GroupID == groupId
                                         select new PagePermissionsObj
                                         {
                                             PageId = pp.PageId,
                                             PageName = p.PageName,
                                             PermissionId = pp.Permission.Id,
                                             PermissionName = pp.PermissionName
                                         }).ToListAsync();

            return pagePermissions;
        }
        public async Task<int> UpdatePagePermissions(int groupId, List<PagePermission> newPermissions)
        {
            try
            {
                var existingPermissions = await _context.PagePermissions
                    .Where(pp => pp.GroupID == groupId)
                    .ToListAsync();

                _context.PagePermissions
                    .RemoveRange(existingPermissions);

                await _context.PagePermissions
                    .AddRangeAsync(newPermissions);
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch(Exception ex)
            {
                throw ;
            }
        }
        public async Task<(IEnumerable<Page> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.Pages
                .Where(x => x.PageName.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
    }
}
