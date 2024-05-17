using Microsoft.EntityFrameworkCore.Metadata;
using Store.Data.Entities;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? Id);
        Task<IReadOnlyList<TEntity>> GetAllAsync(); 
        Task<TEntity> GetWithSpecificationsByIdAsync(ISpecification<TEntity> specs);
        Task<IReadOnlyList<TEntity>> GetWithSpecificationsAllAsync(ISpecification<TEntity> specs); 

        Task<int> CountSpecificationAsync(ISpecification<TEntity> specs);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
