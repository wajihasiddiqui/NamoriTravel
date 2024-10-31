using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public GroupService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GroupDTO>> GetAllGroupAsync(int? userId)
        {
            try
            {
                var groups = await _Repository.GroupRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<GroupDTO>>(groups);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all groups", userId);
                throw;
            }
        }
        public async Task<GroupDTO> GetGroupByIdAsync(int id, int? userId)
        {
            try
            {
                var group = await _Repository.GroupRepository.GetByIdAsync(id);
                return _mapper.Map<GroupDTO>(group);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching group with ID {id}", userId);
                throw;
            }
        }
        public async Task AddGroupAsync(GroupDTO dto, int? userId)
        {
            try
            {
                var group = _mapper.Map<Groups>(dto);
                await _Repository.GroupRepository.AddAsync(group);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "GroupService", "AddGroup", $"Added group with ID {group.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new group", userId);
                throw;
            }
        }
        public async Task UpdateGroupAsync(GroupDTO dto, int? userId)
        {
            try
            {
                var group = _mapper.Map<Groups>(dto);
                group.ModifiedDate = DateTime.UtcNow;
                await _Repository.GroupRepository.UpdateAsync(group);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "GroupService", "UpdateGroup", $"Updated group with ID {group.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating group with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteGroupAsync(GroupDTO dto, int? userId)
        {
            try
            {
                var group = _mapper.Map<Groups>(dto);
                group.IsDeleted = true;
                group.ModifiedDate = DateTime.UtcNow;
                await _Repository.GroupRepository.UpdateAsync(group);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "GroupService", "DeleteGroup", $"Deleted group with ID {group.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting group with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<GroupDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.GroupRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<GroupDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.GroupRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<GroupDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Groups", userId);
                throw;
            }
        }
    }
}
