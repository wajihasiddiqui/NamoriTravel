using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;
using Azure;

namespace ServiceLayer.Services
{
    public class PageService : IPageService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        public PageService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
            
        }

        public async Task<IEnumerable<PageDTO>> GetAllPagesAsync(int? userId)
        {
            try
            {
                var Page = await _Repository.PageRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<PageDTO>>(Page);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all pages", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<PageDTO> pageDTOs, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn,int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.PageRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<PageDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var Page = await _Repository.PageRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<PageDTO>>(Page.Items), Page.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all pages", userId);
                throw;
            }
        }

        public async Task<PageDTO> GetPageByIdAsync(int id, int? userId)
        {
         
            try
            {
                var Page = await _Repository.PageRepository.GetByIdAsync(id);
                return _mapper.Map<PageDTO>(Page);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching page with ID {id}", userId);
                throw;
            }
        }

        public async Task AddPageAsync(PageDTO Dto, int? userId)
        {
            try
            {
                var page = _mapper.Map<Page>(Dto);
                page.CreatedDate = DateTime.UtcNow;
                page.CreatedBy = userId;
                page.IsActive = true;
                if (Dto.ParentPageId == 0)
                {
                    page.ParentPageId = null;
                    page.PageURL = "#";
                }
                await _Repository.PageRepository.AddAsync(page);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PageService", "AddPage", $"Added page with ID {page.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new page", userId);
                throw;
            }
        }

        public async Task UpdatePageAsync(PageDTO Dto, int? userId)
        {
            
            try
            {
                var Page = _mapper.Map<Page>(Dto);
                Page.ModifiedDate = DateTime.UtcNow;
                Page.ModifiedBy = userId;
                if (Dto.ParentPageId == 0)
                    Page.ParentPageId = null;

                await _Repository.PageRepository.UpdateAsync(Page);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PageService", "UpdatePage", $"Updated page with ID {Page.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating page with ID {Dto.Id}", userId);
                throw;
            }
        }

        public async Task DeletePageAsync(PageDTO Dto, int? userId)
        {
            try
            {
                var Page = _mapper.Map<Page>(Dto);
                Page.IsDeleted = true;
                Page.ModifiedDate = DateTime.UtcNow;
                Page.ModifiedBy = userId;
                await _Repository.PageRepository.UpdateAsync(Page);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PageService", "DeletePage", $"Deleted page with ID {Page.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting page with ID {Dto.Id}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<PagePermissionsObjDTO>> GetPagePermissionsByGroupId(int groupId,int? userId)
        {
            try
            {
                var pagePermissions = await _Repository.PageRepository.GetPagePermissionsByGroupId(groupId);
                return _mapper.Map<List<PagePermissionsObjDTO>>(pagePermissions);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching page permissions for group ID {groupId}", userId);
                throw;
            }
        }

        public async Task UpdatePagePermissions(int groupId, List<PagePermissionDTO> newPermissions , int? userId)
        {
            try
            {
                var List  = _mapper.Map<List<PagePermission>>(newPermissions);
                List.ForEach(item =>
                {
                    item.GroupID = groupId;
                    item.CreatedBy = userId;
                    item.CreatedDate = DateTime.UtcNow;
                    item.ModifiedDate = DateTime.UtcNow;
                    item.ModifiedBy = userId;
                });
                await _Repository.PageRepository.UpdatePagePermissions(groupId, List);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "PageService", "UpdatePagePermissions", $"Updated permissions for group ID {groupId}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating page permissions for group ID {groupId}", userId);
                throw;
            }
        }
        
    }
}
