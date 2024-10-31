using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IPageService
    {
        Task<IEnumerable<PageDTO>> GetAllPagesAsync(int? userId);
        Task<PageDTO> GetPageByIdAsync(int id, int? userId);
        Task AddPageAsync(PageDTO GroupDto, int? userId);
        Task UpdatePageAsync(PageDTO GroupDto, int? userId);
        Task DeletePageAsync(PageDTO GroupDto, int? userId);
        Task<IEnumerable<PagePermissionsObjDTO>> GetPagePermissionsByGroupId(int groupId, int? userId);
        Task UpdatePagePermissions(int groupId, List<PagePermissionDTO> newPermissions, int? userId);
        Task<(IEnumerable<PageDTO> pageDTOs, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn,int sortcolval, string sortOrder, int page, int pageSize);

    }
}
