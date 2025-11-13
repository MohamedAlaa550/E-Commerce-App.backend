using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync (bool asNoTracking = false);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity,Tkey> specifications);

        Task<TEntity?> GetAsync(Tkey id);
        Task<TEntity?> GetAsync(ISpecifications<TEntity, Tkey> specifications);
        Task AddAsync (TEntity entity);
        void Update (TEntity entity);
        void Delete (TEntity entity);
        Task<int> ApplayCountAsync (ISpecifications<TEntity,Tkey> specifications);
    }
}
