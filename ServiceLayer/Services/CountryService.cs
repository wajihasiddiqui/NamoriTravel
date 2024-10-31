using ServiceLayer.ServiceInterfaces;
using DomainLayer.Entities;
using DomainLayer;
using AutoMapper;
using ModelsDTO;
using NetTopologySuite.Index.HPRtree;

namespace ServiceLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public CountryService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CountryDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var Countrys = await _Repository.CountryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CountryDTO>>(Countrys);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all Country", userId);
                throw;
            }
        }
        public async Task<CountryDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var Country = await _Repository.CountryRepository.GetByIdAsync(id);
                return _mapper.Map<CountryDTO>(Country);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching Country with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(CountryDTO dto, List<CountryDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Country = _mapper.Map<List<Country>>(DtoList);
                    foreach (var item in Country)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                    }
                    await _Repository.CountryRepository.BulkInsertAsync(Country);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "CountryService", "AddCountry", $"Added Country");
                }
                else
                {
                    var Country = _mapper.Map<Country>(dto);
                    Country.CreatedDate = DateTime.UtcNow;
                    Country.CreatedBy = userId;
                    Country.IsDeleted = false;
                    await _Repository.CountryRepository.AddAsync(Country);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "CountryService", "AddCountry", $"Added Country with ID {Country.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new Country", userId);
                throw;
            }
        }
        public async Task UpdateAsync(CountryDTO dto, List<CountryDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Countrylist = _mapper.Map<List<Country>>(DtoList);
                    foreach (var DTO in Countrylist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CountryRepository.BulkUpdateAsync(Countrylist);
                }
                else
                {
                    var Country = _mapper.Map<Country>(dto);
                    Country.ModifiedDate = DateTime.UtcNow;
                    Country.ModifiedBy = userId;
                    await _Repository.CountryRepository.UpdateAsync(Country);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "CountryService", "UpdateCountry", $"Updated Country");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating Country with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(CountryDTO dto, List<CountryDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var Country = _mapper.Map<Country>(dto);
                    Country.ModifiedDate = DateTime.UtcNow;
                    Country.IsDeleted = true;
                    await _Repository.CountryRepository.UpdateAsync(Country);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var Countrylist = _mapper.Map<List<Country>>(DtoList);
                    foreach (var DTO in Countrylist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CountryRepository.BulkUpdateAsync(Countrylist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "CountryService", "DeleteCountry", $"Deleted Country ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting Country with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<CountryDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.CountryRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<CountryDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.CountryRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<CountryDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync Countrys", userId);
                throw;
            }
        }

    }
    public class CityService : ICityService
    {
        private readonly IRepositoryManager _Repository;
        private readonly IMapper _mapper;
        public CityService(IRepositoryManager Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CityDTO>> GetAllAsync(int? userId)
        {
            try
            {
                var citys = await _Repository.CityRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CityDTO>>(citys);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all city", userId);
                throw;
            }
        } 
        public async Task<IEnumerable<CityDTO>> GetAllByCountryCodeAsync(int? userId,string Code)
        {
            try
            {
                var citys = await _Repository.CityRepository.GetAllByCountryCodeAsync(Code);
                return _mapper.Map<IEnumerable<CityDTO>>(citys);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching all city", userId);
                throw;
            }
        }
        public async Task<CityDTO> GetByIdAsync(int id, int? userId)
        {
            try
            {
                var city = await _Repository.CityRepository.GetByIdAsync(id);
                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error fetching city with ID {id}", userId);
                throw;
            }
        }
        public async Task AddAsync(CityDTO dto, List<CityDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var city = _mapper.Map<List<City>>(DtoList);
                    foreach (var item in city)
                    {
                        item.CreatedDate = DateTime.UtcNow;
                        item.CreatedBy = userId;
                        item.IsDeleted = false;
                        item.IsActive = true;
                    }
                    await _Repository.CityRepository.BulkInsertAsync(city);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "cityService", "Addcity", $"Added city");
                }
                else
                {
                    var city = _mapper.Map<City>(dto);
                    city.CreatedDate = DateTime.UtcNow;
                    city.CreatedBy = userId;
                    city.IsDeleted = false;
                    city.IsActive = true;
                    await _Repository.CityRepository.AddAsync(city);
                    await _Repository.AuditLogRepository.LogAuditAsync(userId, "cityService", "Addcity", $"Added city with ID {city.Id}");
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error adding new city", userId);
                throw;
            }
        }
        public async Task UpdateAsync(CityDTO dto, List<CityDTO> DtoList, int? userId)
        {
            try
            {
                if (DtoList != null && DtoList.Count > 0)
                {
                    var citylist = _mapper.Map<List<City>>(DtoList);
                    foreach (var DTO in citylist)
                    {
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CityRepository.BulkUpdateAsync(citylist);
                }
                else
                {
                    var city = _mapper.Map<City>(dto);
                    city.ModifiedDate = DateTime.UtcNow;
                    city.ModifiedBy = userId;
                    await _Repository.CityRepository.UpdateAsync(city);
                }
                await _Repository.AuditLogRepository.LogAuditAsync(userId, "cityService", "Updatecity", $"Updated city");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error updating city with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task DeleteAsync(CityDTO dto, List<CityDTO> DtoList, int? userId)
        {
            try
            {
                if (dto != null)
                {
                    var city = _mapper.Map<City>(dto);
                    city.ModifiedDate = DateTime.UtcNow;
                    city.IsDeleted = true;
                    await _Repository.CityRepository.UpdateAsync(city);
                }
                if (DtoList != null && DtoList.Count > 0)
                {
                    var citylist = _mapper.Map<List<City>>(DtoList);
                    foreach (var DTO in citylist)
                    {
                        DTO.IsDeleted = true;
                        DTO.CreatedDate = DateTime.UtcNow;
                        DTO.ModifiedBy = userId;
                    }
                    await _Repository.CityRepository.BulkUpdateAsync(citylist);
                }

                await _Repository.AuditLogRepository.LogAuditAsync(userId, "cityService", "Deletecity", $"Deleted city ");
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, $"Error deleting city with ID {dto.Id}", userId);
                throw;
            }
        }
        public async Task<(IEnumerable<CityDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize)
        {
            try
            {
                if (Common.Common.IsStringValue(search))
                {
                    var Result = await _Repository.CityRepository.GetByNameAsync(search);
                    return (_mapper.Map<IEnumerable<CityDTO>>(Result.Items), Result.Total);
                }
                else
                {
                    var data = await _Repository.CityRepository.GetAllByFilteredAsync(userId, search, sortColumn, sortColval, sortOrder, page, pageSize);
                    return (_mapper.Map<IEnumerable<CityDTO>>(data.Items), data.TotalCount);
                }
            }
            catch (Exception ex)
            {
                await _Repository.ErrorLogRepository.LogErrorAsync(ex, "Error fetching GetAllFilteredAsync citys", userId);
                throw;
            }
        }

    }
}
