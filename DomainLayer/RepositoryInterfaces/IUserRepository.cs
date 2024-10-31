using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username,string Email ,string pasword);
        Task<List<PagePermissionsRights>> GetPermissionsForUserByIdAsync(int id);
        Task<(IEnumerable<User> Items, int Total)> GetUserByNameAsync(string SerachValue);
    }
}
