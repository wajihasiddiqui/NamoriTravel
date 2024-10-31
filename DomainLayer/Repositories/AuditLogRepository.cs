using DomainLayer.RepositoryInterfaces;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public AuditLogRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }

        public async Task LogAuditAsync(int? UserID, string? Source, string? action, string? details)
        {
            var auditLog = new AuditLog
            {
                UserId = UserID,
                Source = Source,
                Action = action,
                Details = details,
                CreatedDate = DateTime.UtcNow
            };
            await _context.AddAsync(auditLog);
        }
       

    }
}
