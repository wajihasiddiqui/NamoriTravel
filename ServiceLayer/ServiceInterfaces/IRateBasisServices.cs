using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
   
    public interface IRateBasisServices
    {
        Task<IEnumerable<RateBasisDTO>> GetAllAsync(int? userId);
        Task<RateBasisDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(RateBasisDTO Dto, List<RateBasisDTO> DtoList, int? userId);
        Task UpdateAsync(RateBasisDTO Dto, List<RateBasisDTO> DtoList, int? userId);
        Task DeleteAsync(RateBasisDTO Dto, List<RateBasisDTO> DtoList, int? userId);
        Task<(IEnumerable<RateBasisDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
    }
    public interface IBusinessServices
    {
        Task<IEnumerable<BusinessDTO>> GetAllAsync(int? userId);
        Task<BusinessDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(BusinessDTO Dto, List<BusinessDTO> DtoList, int? userId);
        Task UpdateAsync(BusinessDTO Dto, List<BusinessDTO> DtoList, int? userId);
        Task DeleteAsync(BusinessDTO Dto, List<BusinessDTO> DtoList, int? userId);
        Task<(IEnumerable<BusinessDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
    }

    public interface ICurrencyServices
    {
        Task<IEnumerable<CurrencyDTO>> GetAllAsync(int? userId);
        Task<CurrencyDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(CurrencyDTO Dto, List<CurrencyDTO> DtoList, int? userId);
        Task UpdateAsync(CurrencyDTO Dto, List<CurrencyDTO> DtoList, int? userId);
        Task DeleteAsync(CurrencyDTO Dto, List<CurrencyDTO> DtoList, int? userId);
        Task<(IEnumerable<CurrencyDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
    }

    public interface IAmenitiesService
    {
        Task<IEnumerable<AmenitiesDTO>> GetAllAsync(int? userId);
        Task<AmenitiesDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(AmenitiesDTO Dto, List<AmenitiesDTO> DtoList, int? userId);
        Task UpdateAsync(AmenitiesDTO Dto, List<AmenitiesDTO> DtoList, int? userId);
        Task DeleteAsync(AmenitiesDTO Dto, List<AmenitiesDTO> DtoList, int? userId);
        Task<(IEnumerable<AmenitiesDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);

    }
}
