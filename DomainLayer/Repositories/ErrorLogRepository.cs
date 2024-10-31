using DomainLayer.RepositoryInterfaces;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class ErrorLogRepository: GenericRepository<ErrorLog>, IErrorLogRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public ErrorLogRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task LogErrorAsync(Exception? ex, string? message, int? userId)
        {
            var errorLog = new ErrorLog
            {
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                Message = message,
                ExceptionMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Source = ex.Source
            };
            await _context.AddAsync(errorLog);
        }
    }
}
