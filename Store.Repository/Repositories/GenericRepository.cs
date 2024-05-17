using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using Store.Data.StoreDbContext;
using Store.Repository.Interfaces;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(TEntity entity)
            => await context.Set<TEntity>().AddAsync(entity);

        public async Task<TEntity> GetByIdAsync(TKey? Id)
            => await context.Set<TEntity>().FindAsync(Id);
        public void Delete(TEntity entity)
            => context.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await context.Set<TEntity>().ToListAsync();

        public void Update(TEntity entity)
            => context.Set<TEntity>().Update(entity);

        public async Task<TEntity> GetWithSpecificationsByIdAsync(ISpecification<TEntity> specs)
            => await ApplySpecs(specs).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<TEntity>> GetWithSpecificationsAllAsync(ISpecification<TEntity> specs)
            => await ApplySpecs(specs).ToListAsync();

        public async Task<int> CountSpecificationAsync(ISpecification<TEntity> specs)
            => await ApplySpecs(specs).CountAsync();

        private IQueryable<TEntity> ApplySpecs(ISpecification<TEntity> specs)
            => SpecificationEvaluater<TEntity, TKey>.GetQuery(context.Set<TEntity>(), specs);
    }
}
