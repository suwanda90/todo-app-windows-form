using ApplicationCore.Helpers.BaseEntity.Model;
using ApplicationCore.Helpers.Datatables.Model;
using ApplicationCore.Interfaces.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.BaseEntity
{
    public interface IEfRepository<TEntity, TId> where TEntity : class, IModel<TId>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecificationQuery<TEntity> spec);

        Task<DatatablesPagedResults<TEntity>> GetByPagingAsync(IReadOnlyList<TEntity> data, int start, int length);

        Task<DatatablesPagedResults<TEntity>> DatatablesAsync(DatatablesParameter parameter);

        Task<DatatablesPagedResults<TEntity>> DatatablesAsync(DatatablesParameter parameter, ISpecificationQuery<TEntity> spec);

        Task<TEntity> GetAsync(ISpecificationQuery<TEntity> spec);

        Task<TEntity> GetAsync(ISpecificationQuery<TEntity> spec, TId id);

        Task<TEntity> GetAsync(TId id);

        Task<int> CountAsync();

        Task<int> CountAsync(ISpecificationQuery<TEntity> spec);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<bool> IsExistDataAsync(IDictionary<string, object> where);

        Task<bool> IsExistDataWithKeyAsync(ExistWithKeyModel model);
    }
}
