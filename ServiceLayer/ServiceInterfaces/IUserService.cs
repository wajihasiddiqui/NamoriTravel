using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync(int? userId);
        Task<UserDTO> GetUserByIdAsync(int id, int? userId);
        Task AddUserAsync(UserDTO userDto, int? userId);
        Task UpdateUserAsync(UserDTO userDto, int? userId);
        Task DeleteUserAsync(UserDTO userDto, int? userId);
        Task<(IEnumerable<UserDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
    }
}
