using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int? id);

        Task<bool> InsertAsync(TEntity entity, bool publishEvent = true);

        void Update(TEntity entity, bool publishEvent = true);

        void Delete(TEntity entity, bool publishEvent = true);

        IQueryable<TEntity> Table { get; }
    }
}
