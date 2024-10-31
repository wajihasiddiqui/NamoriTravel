using DomainLayer.GenericRepository;
using DomainLayer.Entities;


namespace DomainLayer.RepositoryInterfaces
{
    public interface IDotwRequestRepository : IGenericRepository<DotwRequest>
    {
        Task<(IEnumerable<DotwRequest> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
