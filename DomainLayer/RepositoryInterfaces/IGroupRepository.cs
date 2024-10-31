using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IGroupRepository: IGenericRepository<Groups>
    {
        Task<(IEnumerable<Groups> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
