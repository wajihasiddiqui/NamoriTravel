using Microsoft.EntityFrameworkCore;
using DomainLayer.GenericRepository;
using DomainLayer.DbContexts;
using EFCore.BulkExtensions;
using DomainLayer.Entities;

namespace DomainLayer.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IActivatable
    {
        private readonly NamoriTrvl_dbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(NamoriTrvl_dbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted).ToListAsync();
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetAllByFilteredAsync(
 int? userId, string search, string sortColumn, int sortColvale, string sortOrder, int page, int pageSize)
        {
            int totalCount = await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted).AsQueryable().CountAsync();
            //int currentPage = Math.Max(page, 1);
            //int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            //currentPage = Math.Min(currentPage, totalPages);
            //int skip = ((currentPage - 1) < 0 ? 0 : (currentPage - 1)) * pageSize;

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                var query = await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted && entity.Id == (Convert.ToInt32(search)))
                    .Skip(page)
                    .Take(pageSize)
                    .ToListAsync();
                return (query, totalCount);
            }

            // Apply sorting
            string[] arr = sortColumn.Split(",");
            string Col = arr[sortColvale - 1];
            if (Col.Length > 1)
            {
                if (sortOrder.ToLower() == "asc")
                {
                    try
                    {
                        var query = await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted)
                        .Skip(page)
                        .Take(pageSize)
                        .OrderBy(e => EF.Property<object>(e, Col))
                        .ToListAsync();
                        return (query, totalCount);
                    }
                    catch (Exception ex) {
                        string str = ex.Message;
                        return (null, 0);
                    }
                }
                else
                {
                    var query = await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted)
                    .Skip(page)
                    .Take(pageSize)
                    .OrderByDescending(e => EF.Property<object>(e, Col))
                    .ToListAsync();
                    return (query, totalCount);
                }
            }
            else
            {
                var query = await _dbSet.Where(entity => entity.IsActive && !entity.IsDeleted)
                .Skip(page)
                .Take(pageSize)
                .ToListAsync();
                return (query, totalCount);
            }
        }



        public async Task<TEntity> GetByIdAsync(int id)
        {
            var Result = await _dbSet.FirstOrDefaultAsync(u => u.Id == id && u.IsActive && !u.IsDeleted);
            return Result;
        }
        public async Task UpdateAsync(TEntity entity)
        {
            // Retrieve the entity to update
            var existingEntity = await _dbSet.FindAsync(GetEntityKey(entity));
            if (existingEntity == null)
            {
                throw new InvalidOperationException("Entity to update was not found.");
            }

            // Update the existing entity's values
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Mark the entity as modified
            _context.Entry(existingEntity).State = EntityState.Modified;

            // Save changes to the context
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet
                .FirstOrDefaultAsync(entity => entity.IsActive && !entity.IsDeleted && entity.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Entity to delete was not found.");
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        private object GetEntityKey(TEntity entity)
        {
            var keyProperty = _context.Model.FindEntityType(typeof(TEntity))
                .FindPrimaryKey().Properties.FirstOrDefault();

            if (keyProperty == null)
            {
                throw new InvalidOperationException("Entity type does not have a primary key.");
            }

            return keyProperty.PropertyInfo.GetValue(entity);
        }
        public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            await _context.BulkInsertAsync(entities.ToList());
        }
        public async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            await _context.BulkDeleteAsync<TEntity>(entities.ToList());
        }
        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            await _context.BulkUpdateAsync<TEntity>(entities.ToList());
        }

    }
}
