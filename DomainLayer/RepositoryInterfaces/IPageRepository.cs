using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IPageRepository : IGenericRepository<Page>
    {
        Task<IEnumerable<PagePermissionsObj>> GetPagePermissionsByGroupId(int groupId);
        Task<int> UpdatePagePermissions(int groupId, List<PagePermission> newPermissions);
        Task<(IEnumerable<Page> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
