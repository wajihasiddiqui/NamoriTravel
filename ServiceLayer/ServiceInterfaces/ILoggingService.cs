
namespace ServiceLayer.ServiceInterfaces
{
    public interface ILoggingService
    {
        Task LogAuditAsync(int? UserID, string? Source, string? action, string? details);
        Task LogErrorAsync(Exception? ex, string? message, int? userId);
    }
}
