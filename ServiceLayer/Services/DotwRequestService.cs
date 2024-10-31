using AutoMapper;
using DomainLayer.Entities;
using DomainLayer;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer.Services
{
    public class DotwRequestService: IDotwRequestService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public DotwRequestService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DotwRequestDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.DotwRequestRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<DotwRequestDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all DotwRequests", userId);
                throw;
            }
        }
        public async Task<DotwRequestDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.DotwRequestRepository.GetByIdAsync(id);
                return _mapper.Map<DotwRequestDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching DotwRequest with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(DotwRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<DotwRequest>(dto);
                Result.CreatedDate = DateTime.UtcNow;
                Result.CreatedBy = userId;
                Result.IsActive = true;
                Result.IsDeleted = false;
                await _Repository.DotwRequestRepository.AddAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "DotwRequestService", "AddDotwRequest", $"Added DotwRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new DotwRequest", userId);
                throw;
            }
        }
        public async Task UpdateAsync(DotwRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<DotwRequest>(dto);
                Result.ModifiedDate = DateTime.UtcNow;
                Result.ModifiedBy = userId;
                Result.IsActive = true;
                Result.IsDeleted = false;
                await _Repository.DotwRequestRepository.UpdateAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "DotwRequestService", "UpdateDotwRequest", $"Updated DotwRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating DotwRequest with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(DotwRequestDTO dto, int? userId)
        {
            try
            {
                var Result = _mapper.Map<DotwRequest>(dto);
                Result.IsDeleted = true;
                Result.ModifiedDate = DateTime.UtcNow;
                Result.ModifiedBy = userId;
                await _Repository.DotwRequestRepository.UpdateAsync(Result);

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "DotwRequestService", "DeleteDotwRequest", $"Deleted DotwRequest with ID {Result.Id}");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting DotwRequest with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<DotwRequestDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.DotwRequestRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<DotwRequestDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.DotwRequestRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<DotwRequestDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all DotwRequests", userId);
                throw;
            }
        }

    }
}
