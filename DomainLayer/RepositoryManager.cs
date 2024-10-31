using DomainLayer.RepositoryInterfaces;
using DomainLayer.Repositories;
using DomainLayer.DbContexts;

namespace DomainLayer
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly NamoriTrvl_dbContext _context;

        private IUserRepository _userRepository;
        private IAuditLogRepository _auditLogRepository;
        private IErrorLogRepository _logErrorRepository;
        private IGroupRepository _groupRepository;
        private IPermissionRepository _permissionRepository;
        private IPageRepository _pageRepository;
        private IRoleRepository _roleRepository;
        private IDepartmentRepository _departmentRepository;
        private IXmlRequestRepository _xmlRequestRepository;
        private IDotwRequestRepository _dotwRequestRepository;
        private ICountryRepository _countryRepository;
        private ICityRepository _cityRepository;
        private IAmenitiesRepository _amenitiesRepository;
        private ICurrencyRepository _currencyRepository;
        private IRateBasisRepository _rateBasisRepository;
        private IBusinessRepository _baninessRepository;

        public RepositoryManager(NamoriTrvl_dbContext context) => _context = context;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IAuditLogRepository AuditLogRepository => _auditLogRepository ?? (_auditLogRepository = new AuditLogRepository(_context));
        public IErrorLogRepository ErrorLogRepository => _logErrorRepository ?? (_logErrorRepository = new ErrorLogRepository(_context));
        public IGroupRepository GroupRepository => _groupRepository ?? (_groupRepository = new GroupRepository(_context));
        public IPermissionRepository PermissionRepository => _permissionRepository ?? (_permissionRepository = new PermissionRepository(_context));
        public IPageRepository PageRepository => _pageRepository ?? (_pageRepository = new PageRepository(_context));
        public IRoleRepository RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_context));
        public IDepartmentRepository DepartmentRepository => _departmentRepository ?? (_departmentRepository = new DepartmentRepository(_context));
        public IDotwRequestRepository DotwRequestRepository => _dotwRequestRepository ?? (_dotwRequestRepository = new DotwRequestRepository(_context));
        public IXmlRequestRepository XmlRequestRepository => _xmlRequestRepository ?? (_xmlRequestRepository = new XmlRequestRepository(_context));
        public ICountryRepository CountryRepository => _countryRepository ?? (_countryRepository = new CountryRepository(_context));
        public ICityRepository CityRepository => _cityRepository ?? (_cityRepository = new CityRepository(_context));
        public IRateBasisRepository RateBasisRepository => _rateBasisRepository ?? (_rateBasisRepository = new RateBasisRepository(_context));
        public ICurrencyRepository CurrencyRepository => _currencyRepository ?? (_currencyRepository = new CurrencyRepository(_context));
        public IBusinessRepository BusinessRepository => _baninessRepository ?? (_baninessRepository = new BusinessRepository(_context));
        public IAmenitiesRepository AmenitiesRepository => _amenitiesRepository ?? (_amenitiesRepository = new AmenitiesRepository(_context));

    }
}
