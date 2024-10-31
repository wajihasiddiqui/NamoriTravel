using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IRateBasisRepository:IGenericRepository<RateBasis>
    {
        Task<(IEnumerable<RateBasis> Items, int Total)> GetByNameAsync(string SerachValue);
    }
    public interface IBusinessRepository : IGenericRepository<Busines>
    {
        Task<(IEnumerable<Busines> Items, int Total)> GetByNameAsync(string SerachValue);
    }
    public interface IAmenitiesRepository : IGenericRepository<Amenities>
    {
        Task<(IEnumerable<Amenities> Items, int Total)> GetByNameAsync(string SerachValue);
    }
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        Task<(IEnumerable<Currency> Items, int Total)> GetByNameAsync(string SerachValue);
    }
}
