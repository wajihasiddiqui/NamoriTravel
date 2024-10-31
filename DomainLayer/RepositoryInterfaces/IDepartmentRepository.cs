using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IDepartmentRepository: IGenericRepository<Department>
    {
        Task<(IEnumerable<Department> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
