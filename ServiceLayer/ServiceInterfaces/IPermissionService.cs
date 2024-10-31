using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync(int? userId);
        Task<PermissionDTO> GetPermissionByIdAsync(int id, int? userId);
        Task AddPermissionAsync(PermissionDTO PermissionDto, int? userId);
        Task UpdatePermissionAsync(PermissionDTO PermissionDto, int? userId);
        Task DeletePermissionAsync(PermissionDTO PermissionDto, int? userId);
        Task<(IEnumerable<PermissionDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
    }
}
