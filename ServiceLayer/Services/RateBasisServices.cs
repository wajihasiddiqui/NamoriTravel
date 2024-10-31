using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;

namespace ServiceLayer.Services
{
    public class RateBasisServices : IRateBasisServices
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public RateBasisServices(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RateBasisDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.RateBasisRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RateBasisDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all RateBasis", userId);
                throw;
            }
        }
        public async Task<RateBasisDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.RateBasisRepository.GetByIdAsync(id);
                return _mapper.Map<RateBasisDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching RateBasis with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(RateBasisDTO dto, List<RateBasisDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Result = _mapper.Map<List<RateBasis>>(DtoList);
                    foreach (var item in Result)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                        item.IsActive = true;
                    }
                    await _Repository.RateBasisRepository.BulkInsertAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "RateBasisService", "AddRateBasis", $"Added RateBasis");
                }
                else
                {
                    var Result = _mapper.Map<RateBasis>(dto);
                    Result.CreatedDate = DateTime.UtcNow;
                    Result.CreatedBy = userId;
                    Result.IsDeleted = false;
                    Result.IsActive = true;
                    await _Repository.RateBasisRepository.AddAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "RateBasisService", "AddRateBasis", $"Added RateBasis with ID {Result.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new RateBasis", userId);
                throw;
            }
        }
        public async Task UpdateAsync(RateBasisDTO dto, List<RateBasisDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<RateBasis>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.RateBasisRepository.BulkUpdateAsync(Resultlist);
                }
                else
                {
                    var Result = _mapper.Map<RateBasis>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.ModifiedBy = userId;
                    await _Repository.RateBasisRepository.UpdateAsync(Result);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "RateBasisService", "UpdateRateBasis", $"Updated RateBasis");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating RateBasis with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(RateBasisDTO dto, List<RateBasisDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var Result = _mapper.Map<RateBasis>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.IsDeleted = true;
                    await _Repository.RateBasisRepository.UpdateAsync(Result);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<RateBasis>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.RateBasisRepository.BulkUpdateAsync(Resultlist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "RateBasisService", "DeleteRateBasis", $"Deleted RateBasis ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting RateBasis with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<RateBasisDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.RateBasisRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<RateBasisDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.RateBasisRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<RateBasisDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync RateBasiss", userId);
                throw;
            }
        }

    }
    public class BusinessServices : IBusinessServices
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public BusinessServices(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BusinessDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.BusinessRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<BusinessDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Business", userId);
                throw;
            }
        }
        public async Task<BusinessDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.BusinessRepository.GetByIdAsync(id);
                return _mapper.Map<BusinessDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching Business with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(BusinessDTO dto, List<BusinessDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Result = _mapper.Map<List<Busines>>(DtoList);
                    foreach (var item in Result)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                    }
                    await _Repository.BusinessRepository.BulkInsertAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "BusinessService", "AddBusiness", $"Added Business");
                }
                else
                {
                    var Result = _mapper.Map<Busines>(dto);
                    Result.CreatedDate = DateTime.UtcNow;
                    Result.CreatedBy = userId;
                    Result.IsDeleted = false;
                    await _Repository.BusinessRepository.AddAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "BusinessService", "AddBusiness", $"Added Business with ID {Result.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new Business", userId);
                throw;
            }
        }
        public async Task UpdateAsync(BusinessDTO dto, List<BusinessDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<Busines>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.BusinessRepository.BulkUpdateAsync(Resultlist);
                }
                else
                {
                    var Result = _mapper.Map<Busines>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.ModifiedBy = userId;
                    await _Repository.BusinessRepository.UpdateAsync(Result);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "BusinessService", "UpdateBusiness", $"Updated Business");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating Business with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(BusinessDTO dto, List<BusinessDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var Result = _mapper.Map<Busines>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.IsDeleted = true;
                    await _Repository.BusinessRepository.UpdateAsync(Result);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<Busines>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.BusinessRepository.BulkUpdateAsync(Resultlist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "BusinessService", "DeleteBusiness", $"Deleted Business ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting Business with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<BusinessDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.BusinessRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<BusinessDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.BusinessRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<BusinessDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Businesss", userId);
                throw;
            }
        }

    }
    public class CurrencyServices : ICurrencyServices
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public CurrencyServices(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CurrencyDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.CurrencyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CurrencyDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Currency", userId);
                throw;
            }
        }
        public async Task<CurrencyDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.CurrencyRepository.GetByIdAsync(id);
                return _mapper.Map<CurrencyDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching Currency with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(CurrencyDTO dto, List<CurrencyDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Result = _mapper.Map<List<Currency>>(DtoList);
                    foreach (var item in Result)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                    }
                    await _Repository.CurrencyRepository.BulkInsertAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "CurrencyService", "AddCurrency", $"Added Currency");
                }
                else
                {
                    var Result = _mapper.Map<Currency>(dto);
                    Result.CreatedDate = DateTime.UtcNow;
                    Result.CreatedBy = userId;
                    Result.IsDeleted = false;
                    await _Repository.CurrencyRepository.AddAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "CurrencyService", "AddCurrency", $"Added Currency with ID {Result.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new Currency", userId);
                throw;
            }
        }
        public async Task UpdateAsync(CurrencyDTO dto, List<CurrencyDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<Currency>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CurrencyRepository.BulkUpdateAsync(Resultlist);
                }
                else
                {
                    var Result = _mapper.Map<Currency>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.ModifiedBy = userId;
                    await _Repository.CurrencyRepository.UpdateAsync(Result);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "CurrencyService", "UpdateCurrency", $"Updated Currency");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating Currency with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(CurrencyDTO dto, List<CurrencyDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var Result = _mapper.Map<Currency>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.IsDeleted = true;
                    await _Repository.CurrencyRepository.UpdateAsync(Result);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<Currency>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CurrencyRepository.BulkUpdateAsync(Resultlist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "CurrencyService", "DeleteCurrency", $"Deleted Currency ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting Currency with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<CurrencyDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.CurrencyRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<CurrencyDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.CurrencyRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<CurrencyDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Currencys", userId);
                throw;
            }
        }

    }
    public class AmenitiesServices : IAmenitiesService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public AmenitiesServices(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AmenitiesDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Result = await _Repository.AmenitiesRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<AmenitiesDTO>>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Amenities", userId);
                throw;
            }
        }
        public async Task<AmenitiesDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Result = await _Repository.AmenitiesRepository.GetByIdAsync(id);
                return _mapper.Map<AmenitiesDTO>(Result);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching Amenities with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(AmenitiesDTO dto, List<AmenitiesDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Result = _mapper.Map<List<DomainLayer.Entities.Amenities>>(DtoList);
                    foreach (var item in Result)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                    }
                    await _Repository.AmenitiesRepository.BulkInsertAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "AmenitiesService", "AddAmenities", $"Added Amenities");
                }
                else
                {
                    var Result = _mapper.Map<DomainLayer.Entities.Amenities>(dto);
                    Result.CreatedDate = DateTime.UtcNow;
                    Result.CreatedBy = userId;
                    Result.IsDeleted = false;
                    await _Repository.AmenitiesRepository.AddAsync(Result);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "AmenitiesService", "AddAmenities", $"Added Amenities with ID {Result.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new Amenities", userId);
                throw;
            }
        }
        public async Task UpdateAsync(AmenitiesDTO dto, List<AmenitiesDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<DomainLayer.Entities.Amenities>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.AmenitiesRepository.BulkUpdateAsync(Resultlist);
                }
                else
                {
                    var Result = _mapper.Map<DomainLayer.Entities.Amenities>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.ModifiedBy = userId;
                    await _Repository.AmenitiesRepository.UpdateAsync(Result);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "AmenitiesService", "UpdateAmenities", $"Updated Amenities");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating Amenities with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(AmenitiesDTO dto, List<AmenitiesDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var Result = _mapper.Map<DomainLayer.Entities.Amenities>(dto);
                    Result.ModifiedDate = DateTime.UtcNow;
                    Result.IsDeleted = true;
                    await _Repository.AmenitiesRepository.UpdateAsync(Result);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Resultlist = _mapper.Map<List<DomainLayer.Entities.Amenities>>(DtoList);
                    foreach (var DTO in Resultlist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.AmenitiesRepository.BulkUpdateAsync(Resultlist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "AmenitiesService", "DeleteAmenities", $"Deleted Amenities ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting Amenities with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<AmenitiesDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.AmenitiesRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<AmenitiesDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.AmenitiesRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<AmenitiesDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Amenitiess", userId);
                throw;
            }
        }

    }
}
