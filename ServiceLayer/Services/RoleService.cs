using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public RoleService(IRepositoryManager Repository,IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync(int? userId)
        {
            try
            {
                var roles = await _Repository.RoleRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RoleDTO>>(roles);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all roles", userId);
                throw;
            }
        }
        public async Task<RoleDTO> GetRoleByIdAsync(int id, int? userId)
        {
            try
            {
                var role = await _Repository.RoleRepository.GetByIdAsync(id);
                return _mapper.Map<RoleDTO>(role);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching role with ID {id}", userId);
                throw;
            }
        }
        public async Task AddRoleAsync(RoleDTO dto, int? userId)
        {
            try
            {
                var role = _mapper.Map<Role>(dto);
                role.CreatedDate = DateTime.UtcNow;
                role.CreatedBy = userId;
                await _Repository.RoleRepository.AddAsync(role);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "RoleService", "AddRole", $"Added role with ID {role.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new role", userId);
                throw;
            }
        }
        public async Task UpdateRoleAsync(RoleDTO dto, int? userId)
        {
            try
            {
                var role = _mapper.Map<Role>(dto);
                role.ModifiedDate = DateTime.UtcNow;
                role.ModifiedBy = userId;
                await _Repository.RoleRepository.UpdateAsync(role);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "RoleService", "UpdateRole", $"Updated role with ID {role.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating role with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteRoleAsync(RoleDTO dto, int? userId)
        {
            try
            {
                var role = _mapper.Map<Role>(dto);
                role.IsDeleted = true;
                role.ModifiedDate = DateTime.UtcNow;
                role.ModifiedBy = userId;
                await _Repository.RoleRepository.UpdateAsync(role);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "RoleService", "DeleteRole", $"Deleted role with ID {role.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting role with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<RoleDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.RoleRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<RoleDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.RoleRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<RoleDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Roles", userId);
                throw;
            }
        }
    }
}
