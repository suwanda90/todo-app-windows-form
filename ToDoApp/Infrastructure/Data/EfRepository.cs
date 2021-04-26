using ApplicationCore.Helpers.BaseEntity.Model;
using ApplicationCore.Helpers.Datatables;
using ApplicationCore.Helpers.Datatables.Model;
using ApplicationCore.Interfaces.BaseEntity;
using ApplicationCore.Interfaces.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<TEntity, TId> : IEfRepository<TEntity, TId> where TEntity : class, IModel<TId>
    {
        public readonly ApplicationDbContext _dbContext;

        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecificationQuery<TEntity> spec)
        {
            return await GetAll(spec).ToListAsync();
        }

        public async Task<DatatablesPagedResults<TEntity>> DatatablesAsync(DatatablesParameter parameter)
        {
            var source = GetAll();
            TEntity[] items;
            source = DatatablesHelper.SearchData(source, parameter);
            source = DatatablesHelper.SortData(source, parameter);
            var size = await source.CountAsync();
            if (parameter.Length > 0)
            {
                items = await source
                    .Skip((parameter.Start / parameter.Length) * parameter.Length)
                    .Take(parameter.Length)
                    .ToArrayAsync();
            }
            else
            {
                items = await source
                .ToArrayAsync();
            }

            return new DatatablesPagedResults<TEntity>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<DatatablesPagedResults<TEntity>> DatatablesAsync(DatatablesParameter parameter, ISpecificationQuery<TEntity> spec)
        {
            var source = GetAll();
            TEntity[] items;
            source = DatatablesHelper.SearchData(source, parameter);
            source = DatatablesHelper.SortData(source, parameter);
            var size = await source.CountAsync();
            if (parameter.Length > 0)
            {
                items = await source
                    .Skip((parameter.Start / parameter.Length) * parameter.Length)
                    .Take(parameter.Length)
                    .ToArrayAsync();
            }
            else
            {
                items = await source
                .ToArrayAsync();
            }

            return new DatatablesPagedResults<TEntity>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<DatatablesPagedResults<TEntity>> GetByPagingAsync(IReadOnlyList<TEntity> source, int start, int length)
        {
            var data = source.AsQueryable()
                             .AsNoTracking();

            var size = await data
                .CountAsync();

            TEntity[] items;
            if (start > 0 && length > 0)
            {
                items = await data
                .Skip((start - 1) * length)
                .Take(length)
                .ToArrayAsync();
            }
            else
            {
                items = await data
                .ToArrayAsync();
            }

            return new DatatablesPagedResults<TEntity>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<TEntity> GetAsync(ISpecificationQuery<TEntity> spec)
        {
            return await GetAll(spec).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync(TId id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TEntity> GetAsync(ISpecificationQuery<TEntity> spec, TId id)
        {
            return await GetAll(spec).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<TEntity>()
                                   .AsNoTracking()
                                   .CountAsync();
        }

        public async Task<int> CountAsync(ISpecificationQuery<TEntity> spec)
        {
            return await GetAll(spec).CountAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExistDataAsync(IDictionary<string, object> whereData)
        {
            var dbSet = _dbContext.Set<TEntity>()
                                  .AsQueryable()
                                  .AsNoTracking();

            IQueryable<TEntity> exp;

            var whereCriteria = string.Empty;

            int i = 1;
            foreach (var item in whereData)
            {
                if (item.Value.GetType() == typeof(string) || item.Value.GetType() == typeof(Guid) || item.Value.GetType() == typeof(DateTime))
                {
                    whereCriteria += item.Key + "=\"" + item.Value + "\"";
                }
                else if (item.Value.GetType() == typeof(DateTime))
                {
                    var dateValue = DateTime.Parse(item.Value.ToString());
                    var dateValueAdd = DateTime.Parse(item.Value.ToString()).AddDays(1);

                    whereCriteria += item.Key + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Key + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                }
                else
                {
                    whereCriteria += item.Key + "=" + item.Value;
                }

                if (i < whereData.Count)
                {
                    whereCriteria += " and ";
                }

                i++;
            }

            exp = dbSet.Where(whereCriteria);
            var data = await exp.AnyAsync();
            return data;
        }

        public async Task<bool> IsExistDataWithKeyAsync(ExistWithKeyModel model)
        {
            var dbSet = _dbContext.Set<TEntity>()
                                   .AsQueryable()
                                   .AsNoTracking();

            IQueryable<TEntity> oldData;

            if (model.KeyValue.GetType() == typeof(string) || model.KeyValue.GetType() == typeof(Guid))
            {
                oldData = dbSet.Where(model.KeyName + "=\"" + model.KeyValue + "\"");
            }
            else
            {
                oldData = dbSet.Where(model.KeyName + "=" + model.KeyValue);
            }

            var resulOldtData = oldData.Select(model.FieldName).ToDynamicList();
            var oldValue = resulOldtData[0];

            bool result;

            if (oldValue != (dynamic)model.FieldValue)
            {
                IQueryable<TEntity> exp;

                var whereCriteria = string.Empty;

                int i = 1;
                foreach (var item in model.WhereData)
                {
                    if (item.Value.GetType() == typeof(string) || item.Value.GetType() == typeof(Guid))
                    {
                        whereCriteria += item.Key + "=\"" + item.Value + "\"";
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        var dateValue = DateTime.Parse(item.Value.ToString());
                        var dateValueAdd = DateTime.Parse(item.Value.ToString()).AddDays(1);

                        whereCriteria += item.Key + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Key + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                    }
                    else
                    {
                        whereCriteria += item.Key + "=" + item.Value;
                    }

                    if (i < model.WhereData.Count)
                    {
                        whereCriteria += " and ";
                    }

                    i++;
                }

                exp = dbSet.Where(whereCriteria);
                result = await exp.AnyAsync();
            }
            else
            {
                if (model.WhereData.Count > 1)
                {
                    IQueryable<TEntity> exp;

                    var whereCriteria = string.Empty;

                    int i = 1;
                    foreach (var item in model.WhereData)
                    {
                        if (item.Value.GetType() == typeof(string) || item.Value.GetType() == typeof(Guid))
                        {
                            whereCriteria += item.Key + "=\"" + item.Value + "\"";
                        }
                        else if (item.Value.GetType() == typeof(DateTime))
                        {
                            var dateValue = DateTime.Parse(item.Value.ToString());
                            var dateValueAdd = DateTime.Parse(item.Value.ToString()).AddDays(1);

                            whereCriteria += item.Key + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Key + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                        }
                        else
                        {
                            whereCriteria += item.Key + "=" + item.Value;
                        }

                        if (i < model.WhereData.Count)
                        {
                            whereCriteria += " and ";
                        }

                        i++;
                    }

                    whereCriteria += " and ";

                    if (model.KeyValue.GetType() == typeof(string) || model.KeyValue.GetType() == typeof(Guid))
                    {
                        whereCriteria += model.KeyName + "<> \"" + model.KeyValue + "\"";
                    }
                    else
                    {
                        whereCriteria += model.KeyName + " <> " + model.KeyValue;
                    }

                    exp = dbSet.Where(whereCriteria);
                    result = await exp.AnyAsync();
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public IQueryable<TEntity> GetAll()
        {
            var data = _dbContext.Set<TEntity>()
                                 .AsQueryable()
                                 .AsNoTracking();

            return data;
        }

        public IQueryable<TEntity> GetAll(ISpecificationQuery<TEntity> spec)
        {
            var source = GetAll();
            return BaseSpecificationQuery<TEntity>.GetQuery(source, spec);
        }
    }
}
