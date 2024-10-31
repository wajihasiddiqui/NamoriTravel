using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;

namespace ServiceLayer.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IRepositoryManager _AuditLogRepoisotory;
        private readonly IRepositoryManager _ErrorLogRepoisotory;
        private readonly IMapper _mapper;
        public LoggingService(IRepositoryManager AuditLogRepoisotory, IMapper mapper)
        {
            _AuditLogRepoisotory = AuditLogRepoisotory;
            _ErrorLogRepoisotory = AuditLogRepoisotory;
            _mapper = mapper;
        }
        public async Task LogAuditAsync(int? UserID , string? Source, string? action, string? details)
        {
            var auditLog = new AuditLog
            {
                UserId = UserID,
                Source = Source,
                Action = action,
                Details = details,
                CreatedDate = DateTime.UtcNow
            };
            await _AuditLogRepoisotory.AuditLogRepository.AddAsync(auditLog);
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
            await _ErrorLogRepoisotory.ErrorLogRepository.AddAsync(errorLog);
        }
    }
}
