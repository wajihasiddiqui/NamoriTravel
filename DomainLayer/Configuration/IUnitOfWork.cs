using System.Threading.Tasks;
namespace DomainLayer.Configuration
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
