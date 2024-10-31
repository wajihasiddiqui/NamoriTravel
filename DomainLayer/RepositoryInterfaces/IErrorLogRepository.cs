using DomainLayer.GenericRepository;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IErrorLogRepository: IGenericRepository<ErrorLog>
    {
        Task LogErrorAsync(Exception? ex, string? message, int? userId);
    }
}
