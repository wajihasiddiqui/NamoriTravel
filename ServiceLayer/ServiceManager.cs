using Microsoft.Extensions.Configuration;
using ServiceLayer.ServiceInterfaces;
using ServiceLayer.Services;
using DomainLayer;
using AutoMapper;

namespace ServiceLayer
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _lazyAuthService;
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IDepartmentService> _lazydepartmentService;
        private readonly Lazy<IGroupService> _lazygroupService;
        private readonly Lazy<ILoggingService> _lazyloggingService;
        private readonly Lazy<IPermissionService> _lazypermissionService;
        private readonly Lazy<IRoleService> _lazyroleService;
        private readonly Lazy<IPageService> _lazypageService;
        private readonly Lazy<IXmlRequestService> _lazyxmlRequestService;
        private readonly Lazy<IDotwRequestService> _lazydotwRequestService;
        private readonly Lazy<ICountryService> _lazycountryService;
        private readonly Lazy<ICityService> _lazycityService;
        private readonly Lazy<IAmenitiesService> _lazyamentiesService;
        private readonly Lazy<IBusinessServices> _lazybusinessService;
        private readonly Lazy<ICurrencyServices> _lazycurrencyService;
        private readonly Lazy<IRateBasisServices> _lazyrateBasisServices;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper autoMapper, IConfiguration configuration)
        {
            _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager, autoMapper, configuration));
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager, configuration));
            _lazydepartmentService = new Lazy<IDepartmentService>(() => new DepartmentService(repositoryManager, autoMapper));
            _lazygroupService = new Lazy<IGroupService>(() => new GroupService(repositoryManager, autoMapper));
            _lazyloggingService = new Lazy<ILoggingService>(() => new LoggingService(repositoryManager, autoMapper));
            _lazypermissionService = new Lazy<IPermissionService>(() => new PermissionService(repositoryManager, autoMapper));
            _lazyroleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager, autoMapper));
            _lazypageService = new Lazy<IPageService>(() => new PageService(repositoryManager, autoMapper));
            _lazyxmlRequestService = new Lazy<IXmlRequestService>(() => new XmlRequestService(repositoryManager, autoMapper));
            _lazydotwRequestService = new Lazy<IDotwRequestService>(() => new DotwRequestService(repositoryManager, autoMapper));
            _lazycountryService = new Lazy<ICountryService>(() => new CountryService(repositoryManager, autoMapper));
            _lazycityService = new Lazy<ICityService>(() => new CityService(repositoryManager, autoMapper));
            _lazyamentiesService = new Lazy<IAmenitiesService>(() => new AmenitiesServices(repositoryManager, autoMapper));
            _lazybusinessService = new Lazy<IBusinessServices>(() => new BusinessServices(repositoryManager, autoMapper));
            _lazycurrencyService = new Lazy<ICurrencyServices>(() => new CurrencyServices(repositoryManager, autoMapper));
            _lazyrateBasisServices = new Lazy<IRateBasisServices>(() => new RateBasisServices(repositoryManager, autoMapper));
        }

        public IAuthService authService => _lazyAuthService.Value;
        public IUserService userService => _lazyUserService.Value;
        public IDepartmentService DepartmentService => _lazydepartmentService.Value;
        public IGroupService groupService => _lazygroupService.Value;
        public ILoggingService loggingService => _lazyloggingService.Value;
        public IPermissionService permissionService => _lazypermissionService.Value;
        public IRoleService roleService => _lazyroleService.Value;
        public IPageService pageService => _lazypageService.Value;
        public IDotwRequestService dotwRequestService => _lazydotwRequestService.Value;
        public IXmlRequestService xmlRequestService => _lazyxmlRequestService.Value;
        public ICountryService countryService => _lazycountryService.Value;
        public ICityService cityService => _lazycityService.Value;
        public IRateBasisServices rateBasisServices => _lazyrateBasisServices.Value;
        public ICurrencyServices currencyServices => _lazycurrencyService.Value;
        public IBusinessServices businessServices => _lazybusinessService.Value;
        public IAmenitiesService amenitiesService => _lazyamentiesService.Value;

    }

}
