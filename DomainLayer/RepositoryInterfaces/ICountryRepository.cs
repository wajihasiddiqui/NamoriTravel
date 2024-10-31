using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface ICountryRepository :IGenericRepository<Country>
    {
        Task<(IEnumerable<Country> Items, int Total)> GetByNameAsync(string SerachValue);
    }
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<(IEnumerable<City> Items, int Total)> GetByNameAsync(string SerachValue);
        Task<IEnumerable<City>> GetAllByCountryCodeAsync(string? Code);
    }
}
