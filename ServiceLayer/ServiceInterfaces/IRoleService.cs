using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface  IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync(int? userId);
        Task<RoleDTO> GetRoleByIdAsync(int id, int? userId);
        Task AddRoleAsync(RoleDTO RoleDto, int? userId);
        Task UpdateRoleAsync(RoleDTO RoleDto, int? userId);
        Task DeleteRoleAsync(RoleDTO RoleDto, int? userId);
        Task<(IEnumerable<RoleDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
