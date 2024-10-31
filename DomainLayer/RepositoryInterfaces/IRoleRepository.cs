using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IRoleRepository: IGenericRepository<Role>
    {
        Task<(IEnumerable<Role> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
