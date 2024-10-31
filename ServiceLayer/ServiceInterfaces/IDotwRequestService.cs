using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IDotwRequestService
    {
        Task<IEnumerable<DotwRequestDTO>> GetAllAsync(int? userId);
        Task<DotwRequestDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(DotwRequestDTO Dto, int? userId);
        Task UpdateAsync(DotwRequestDTO Dto, int? userId);
        Task DeleteAsync(DotwRequestDTO Dto, int? userId);
        Task<(IEnumerable<DotwRequestDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
