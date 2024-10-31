using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public UserRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User> GetUserByUsernameAsync(string username,string Email, string pasword)
        {
            var result = await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.Email==Email && x.PasswordHash == pasword && x.IsActive && !x.IsDeleted);
            return result;
        }
        public async Task<(IEnumerable<User> Items, int Total)> GetUserByNameAsync(string SerachValue)
        {
            var result = await _context.Users
                .Where(x => (x.Username.Contains(SerachValue) 
                || x.Email.Contains(SerachValue))
                && x.IsActive 
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
        public async Task<List<PagePermissionsRights>> GetPermissionsForUserByIdAsync(int userId)
        {
            var user = await _context.Users
                             .Include(u => u.Role)
                             .Include(u => u.Group)
                             .Include(u => u.Department)
                             .FirstOrDefaultAsync(u => u.Id == userId &&
                                  u.Role.IsActive && !u.Role.IsDeleted &&
                                  u.Group.IsActive && !u.Group.IsDeleted &&
                                  u.Department.IsActive && !u.Department.IsDeleted);

            if (user == null)
            {
                return new List<PagePermissionsRights>();
            }

            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == user.RoleId)
                .Select(rp => rp.Permission.PermissionName)
                .ToListAsync();

            var groupPermissions = await _context.GroupPermissions
                .Where(gp => gp.Id == user.GroupId && gp.IsActive && !gp.IsDeleted)
                .Select(gp => gp.Permission.PermissionName)
                .ToListAsync();

            var departmentPermissions = await _context.GroupDepartments
                .Where(gd => gd.DepartmentId == user.DepartmentId && gd.IsActive && !gd.IsDeleted)
                .SelectMany(gd => _context.GroupPermissions.Where(gp => gp.Id == gd.Id))
                .Select(gp => gp.Permission.PermissionName)
                .ToListAsync();

            var pagePermissions = await _context.PagePermissions
                .Include(pp => pp.Page)
                .Where(pp => rolePermissions.Contains(pp.Permission.PermissionName) ||
                             groupPermissions.Contains(pp.Permission.PermissionName) ||
                             departmentPermissions.Contains(pp.Permission.PermissionName)
                             && (pp.IsActive && !pp.IsDeleted)
                             && (pp.Page.IsActive && !pp.Page.IsDeleted))
                .Select(pp => new PagePermissionsRights
                {
                    PageId = pp.PageId,
                    PageName = pp.Page.PageName,
                    PageUrl = pp.Page.PageURL,
                    PageParentId = pp.Page.ParentPageId,
                    Permissions = new List<string> { pp.Permission.PermissionName }
                })
                .ToListAsync();

            var groupedPermissions = pagePermissions
                .GroupBy(pp => pp.PageId)
                .Select(g => new PagePermissionsRights
                {
                    PageId = g.Key,
                    PageName = g.First().PageName,
                    PageUrl = g.First().PageUrl,
                    PageParentId = g.First().PageParentId,
                    Permissions = g.SelectMany(pp => pp.Permissions).ToList()
                })
                .ToList();

            return groupedPermissions;
        }
    }
}