using ModelsDTO;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IXmlRequestService
    {
        Task<IEnumerable<XmlRequestDTO>> GetAllAsync(int? userId);
        Task<XmlRequestDTO> GetByIdAsync(int id, int? userId);
        Task AddAsync(XmlRequestDTO Dto, int? userId);
        Task UpdateAsync(XmlRequestDTO Dto, int? userId);
        Task DeleteAsync(XmlRequestDTO Dto, int? userId);
        Task<string> GenerateRequestXmlAsync(string UserType, string requestType, Dictionary<string, string> parameters, int userId);
        Task<(IEnumerable<XmlRequestDTO> DTO, int Total)> GetAllFilteredAsync(int? userId, string search, string sortColumn, int sortColval, string sortOrder, int page, int pageSize);
        Task<string> SendDotWConnectRequestAsync(string requestXml, int userId, string URL);
    }
}
