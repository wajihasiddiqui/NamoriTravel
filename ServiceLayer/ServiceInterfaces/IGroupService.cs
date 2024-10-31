using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDTO>> GetAllGroupAsync(int? userId);
        Task<GroupDTO> GetGroupByIdAsync(int id, int? userId);
        Task AddGroupAsync(GroupDTO GroupDto, int? userId);
        Task UpdateGroupAsync(GroupDTO GroupDto, int? userId);
        Task DeleteGroupAsync(GroupDTO GroupDto, int? userId);
        Task<(IEnumerable<GroupDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
