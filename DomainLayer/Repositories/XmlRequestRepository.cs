using DomainLayer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using DomainLayer.DbContexts;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class XmlRequestRepository : GenericRepository<XmlRequest>, IXmlRequestRepository
    {
        private readonly NamoriTrvl_dbContext _context;
        public XmlRequestRepository(NamoriTrvl_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(IEnumerable<XmlRequest> Items, int Total)> GetByNameAsync(string SerachValue)
        {
            var result = await _context.XmlRequests
                .Where(x => x.RequestType.Contains(SerachValue)
                && x.IsActive
                && !x.IsDeleted)
                .ToListAsync();
            return (result, result.Count());
        }
        public async Task<XmlRequest> GetXmlRequestByTypeAsync(string RequestType)
        {
            var result = await _context.XmlRequests
                .Where(x => x.RequestType.Contains(RequestType)
                && x.IsActive
                && !x.IsDeleted).FirstOrDefaultAsync();

            return result;
        }
    }
}
