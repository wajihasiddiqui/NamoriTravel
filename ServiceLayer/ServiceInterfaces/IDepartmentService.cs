using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentAsync(int? userId);
        Task<DepartmentDTO> GetDepartmentByIdAsync(int id, int? userId);
        Task AddDepartmentAsync(DepartmentDTO Dto, int? userId);
        Task UpdateDepartmentAsync(DepartmentDTO Dto, int? userId);
        Task DeleteDepartmentAsync(DepartmentDTO Dto, int? userId);
        Task<(IEnumerable<DepartmentDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
