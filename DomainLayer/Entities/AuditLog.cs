
namespace DomainLayer.Entities
{
    public class AuditLog : BaseEntity
    {
        public int? UserId { get; set; }
        public string? Action { get; set; }
        public string? Source { get; set; }
        public string? Details { get; set; }
    }

    public class ErrorLog : BaseEntity
    {
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
    }
}
