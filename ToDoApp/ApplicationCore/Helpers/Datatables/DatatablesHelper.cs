using ApplicationCore.Helpers.Datatables.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers.Datatables
{
    public static class DatatablesHelper
    {
        public static async Task<DatatablesPagedResults<TEntity>> DatatablesAsync<TEntity>(this IQueryable<TEntity> source, DatatablesParameter parameter)
        {
            TEntity[] items;
            source = SearchData(source, parameter);
            source = SortData(source, parameter);
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

        public static DatatablesPagedResults<TEntity> Datatables<TEntity>(this IReadOnlyList<TEntity> data, DatatablesParameter parameter)
        {
            var source = data.AsQueryable();
            TEntity[] items;
            source = SearchData(source, parameter);
            source = SortData(source, parameter);

            var size = source.Count();
            if (parameter.Length > 0)
            {
                items = source
                    .Skip((parameter.Start / parameter.Length) * parameter.Length)
                    .Take(parameter.Length)
                    .ToArray();
            }
            else
            {
                items = source
                .ToArray();
            }

            return new DatatablesPagedResults<TEntity>
            {
                Items = items,
                TotalSize = size
            };
        }

        public static IQueryable<TEntity> SortData<TEntity>(IQueryable<TEntity> source, DatatablesParameter parameter)
        {
            var columns = parameter.Columns.ToArray();
            var isThenBy = false;

            if (parameter.Order.Count() > 0)
            {
                foreach (var item in parameter.Order)
                {
                    if (parameter.Columns[item.Column].Orderable)
                    {
                        if (isThenBy)
                        {
                            source = (source as IOrderedQueryable<TEntity>).ThenBy(columns[item.Column].Data + " " + item.Dir.ToString().Trim());
                        }
                        else
                        {
                            source = source.OrderBy(columns[item.Column].Data + " " + item.Dir.ToString().Trim());
                        }

                        isThenBy = true;
                    }
                }
            }

            return source;
        }

        public static IQueryable<TEntity> SearchData<TEntity>(IQueryable<TEntity> source, DatatablesParameter parameter)
        {
            if (!string.IsNullOrEmpty(parameter.Search.Value))
            {
                Type objType = typeof(TEntity);

                var searchCriteria = string.Empty;

                var columns = parameter.Columns.Where(x => x.Searchable == true).ToArray();

                foreach (var item in columns)
                {
                    var fieldType = string.Empty;

                    if (item.Data.Split(".").Count() > 1)
                    {
                        fieldType = "string";
                    }
                    else
                    {
                        fieldType = item.Name;
                    }

                    if (fieldType == "string" || fieldType == "guid")
                    {
                        searchCriteria += item.Data + ".Contains(\"" + parameter.Search.Value + "\")";
                        searchCriteria += " or ";
                    }
                    else if (fieldType == "datetime")
                    {
                        if (parameter.Search.Value.IsDate())
                        {
                            var dateValue = DateTime.Parse(parameter.Search.Value);
                            var dateValueAdd = DateTime.Parse(parameter.Search.Value).AddDays(1);

                            searchCriteria += item.Data + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Data + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                            searchCriteria += " or ";
                        }
                    }
                    else
                    {
                        if (parameter.Search.Value.IsNumber())
                        {
                            searchCriteria += item.Data + "=" + parameter.Search.Value;
                            searchCriteria += " or ";
                        }
                    }
                }

                searchCriteria = searchCriteria.Remove((searchCriteria.Length) - 4, 4);

                source = source.Where(searchCriteria, parameter.Search.Value);
            }

            return source;
        }
    }
}
