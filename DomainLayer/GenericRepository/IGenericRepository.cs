
namespace DomainLayer.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
        Task BulkDeleteAsync(IEnumerable<TEntity> entities);
        Task BulkUpdateAsync(IEnumerable<TEntity> entities);
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetAllByFilteredAsync(
    int? userId, string search, string sortColumn,int sortColvalue, string sortOrder, int page, int pageSize);
    }

}
