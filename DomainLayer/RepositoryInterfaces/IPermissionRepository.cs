using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IPermissionRepository: IGenericRepository<Permission>
    {
        Task<(IEnumerable<Permission> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
