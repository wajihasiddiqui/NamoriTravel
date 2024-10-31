using DomainLayer.GenericRepository;
using DomainLayer.Entities;

namespace DomainLayer.RepositoryInterfaces
{
    public interface IXmlRequestRepository : IGenericRepository<XmlRequest>
    {
        Task<(IEnumerable<XmlRequest> Items, int Total)> GetByNameAsync(string SerachValue);
        Task<XmlRequest> GetXmlRequestByTypeAsync(string RequestType);
    }
}
