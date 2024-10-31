using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IAuditLogRepository: IGenericRepository<AuditLog>
    {
        Task LogAuditAsync(int? UserID, string? Source, string? action, string? details);
    }
}
