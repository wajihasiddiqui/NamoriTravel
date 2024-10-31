using DomainLayer.RepositoryInterfaces;
namespace DomainLayer
{
    public interface IRepositoryManager
    {
        public IAuditLogRepository AuditLogRepository { get; }
        public IErrorLogRepository ErrorLogRepository { get; }
        public IUserRepository UserRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IPageRepository PageRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IPermissionRepository PermissionRepository { get; }
        public IDotwRequestRepository DotwRequestRepository { get; }
        public IXmlRequestRepository XmlRequestRepository { get; }
        public ICountryRepository CountryRepository { get; }
        public ICityRepository CityRepository { get; }
        public IRateBasisRepository RateBasisRepository { get; }
        public IBusinessRepository BusinessRepository { get; }
        public ICurrencyRepository CurrencyRepository { get; }
        public IAmenitiesRepository AmenitiesRepository { get; }
    }
}
