using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ILogger<EntityRepository<TEntity>> _logger;
        private readonly PdfDbContext _databaseContext;
        protected readonly DbSet<TEntity> _table;

        public EntityRepository(
            ILogger<EntityRepository<TEntity>> logger,
            PdfDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _table = _databaseContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(int? id)
        {
            if (!id.HasValue || id == 0)
                return null;

            return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }

        public virtual async Task<bool> InsertAsync(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await _table.AddAsync(entity);
                return await _databaseContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be added: {ex.Message} and {ex.InnerException}");
            }
        }
      
        public virtual void Update(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _table.Update(entity);
                _databaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(TEntity)} could not be updated: {e.Message} and {e.InnerException}");
            }
        }

        public virtual void Delete(TEntity entity, bool publishEvent = true)
        {
            _table.Remove(entity);
            _databaseContext.SaveChanges();
        }

        public virtual IQueryable<TEntity> Table => _table;

    }
}
