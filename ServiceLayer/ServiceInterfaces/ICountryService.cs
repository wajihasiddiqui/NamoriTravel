using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDTO>> GetAllAsync(int? userId);
        Task<CountryDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(CountryDTO Dto, List<CountryDTO> DtoList, int? userId);
        Task UpdateAsync(CountryDTO Dto, List<CountryDTO> DtoList, int? userId);
        Task DeleteAsync(CountryDTO Dto, List<CountryDTO> DtoList, int? userId);
        Task<(IEnumerable<CountryDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
    public interface ICityService
    {
        Task<IEnumerable<CityDTO>> GetAllAsync(int? userId);
        Task<IEnumerable<CityDTO>> GetAllByCountryCodeAsync(int? userId, string Code);
        Task<CityDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(CityDTO Dto, List<CityDTO> DtoList, int? userId);
        Task UpdateAsync(CityDTO Dto, List<CityDTO> DtoList, int? userId);
        Task DeleteAsync(CityDTO Dto, List<CityDTO> DtoList, int? userId);
        Task<(IEnumerable<CityDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
