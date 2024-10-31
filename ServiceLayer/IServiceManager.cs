using ServiceLayer.ServiceInterfaces;
namespace ServiceLayer
{
    public interface IServiceManager
    {
        public IAuthService authService { get; }
        public IUserService userService { get; }
        public IDepartmentService DepartmentService { get; }
        public IGroupService groupService { get; }
        public IPermissionService permissionService { get; }
        public IRoleService roleService { get; }
        public IPageService pageService { get; }
        public ILoggingService loggingService { get; }
        public IDotwRequestService dotwRequestService { get; }
        public IXmlRequestService xmlRequestService { get; }
        public ICountryService countryService { get; }
        public ICityService cityService { get; }
        public IRateBasisServices rateBasisServices { get; }
        public ICurrencyServices currencyServices { get; }
        public IBusinessServices businessServices { get; }
        public IAmenitiesService amenitiesService { get; }
    }
}
