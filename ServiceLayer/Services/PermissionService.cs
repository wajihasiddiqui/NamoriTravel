using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public PermissionService(IRepositoryManager Repository ,IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync(int? userId)
        {
            try
            {
                var permissions = await _Repository.PermissionRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<PermissionDTO>>(permissions);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all permissions", userId);
                throw;
            }
        }
        public async Task<PermissionDTO> GetPermissionByIdAsync(int id, int? userId)
        {
            try
            {
                var permission = await _Repository.PermissionRepository.GetByIdAsync(id);
                return _mapper.Map<PermissionDTO>(permission);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching permission with ID {id}", userId);
                throw;
            }
        }
        public async Task AddPermissionAsync(PermissionDTO dto, int? userId)
        {
            try
            {
                var permission = _mapper.Map<Permission>(dto);
                permission.CreatedBy = userId;
                permission.CreatedDate = DateTime.UtcNow;
                await _Repository.PermissionRepository.AddAsync(permission);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PermissionService", "AddPermission", $"Added permission with ID {permission.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new permission", userId);
                throw;
            }
        }
        public async Task UpdatePermissionAsync(PermissionDTO dto, int? userId)
        {
            try
            {
                var permission = _mapper.Map<Permission>(dto);
                permission.ModifiedDate = DateTime.UtcNow;
                permission.ModifiedBy = userId;
                await _Repository.PermissionRepository.UpdateAsync(permission);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PermissionService", "UpdatePermission", $"Updated permission with ID {permission.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating permission with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeletePermissionAsync(PermissionDTO dto, int? userId)
        {
            try
            {
                var permission = _mapper.Map<Permission>(dto);
                permission.IsDeleted = true;
                permission.ModifiedDate = DateTime.UtcNow;
                permission.ModifiedBy = userId;
                await _Repository.PermissionRepository.UpdateAsync(permission);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PermissionService", "DeletePermission", $"Deleted permission with ID {permission.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting permission with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<PermissionDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.PermissionRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<PermissionDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.PermissionRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<PermissionDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Permissions", userId);
                throw;
            }
        }
    }
}
