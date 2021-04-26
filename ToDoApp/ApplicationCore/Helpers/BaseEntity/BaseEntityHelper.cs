using ApplicationCore.Helpers.BaseEntity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers.BaseEntity
{
    public static class BaseEntityHelper
    {
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static async Task<IQueryable<TEntity>> SearchData<TEntity>(this IQueryable<TEntity> source, IDictionary<string, Type> where, string keyword)
        {
            if (await source.AnyAsync())
            {
                var whereCriteria = string.Empty;

                foreach (var item in where)
                {
                    if (item.Value == typeof(string) || item.Value == typeof(Guid))
                    {
                        whereCriteria += item.Key + ".Contains(\"" + keyword + "\")"; whereCriteria += " or ";
                    }
                    else if (item.Value == typeof(DateTime?) || item.Value == typeof(DateTime))
                    {
                        if (keyword.IsDate())
                        {
                            var dateValue = DateTime.Parse(keyword);
                            var dateValueAdd = DateTime.Parse(keyword).AddDays(1);

                            whereCriteria += item.Key + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Key + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                            whereCriteria += " or ";
                        }
                    }
                    else
                    {
                        if (keyword.IsIntegralTypes(item.Value))
                        {
                            whereCriteria += item.Key + "=" + keyword;
                            whereCriteria += " or ";
                        }
                    }
                }

                whereCriteria = whereCriteria.Remove((whereCriteria.Length) - 4, 4);

                return source.Where(whereCriteria).AsQueryable();
            }

            return source;
        }

        public static async Task<bool> IsExistDataAsync<TEntity>(this IQueryable<TEntity> source, IDictionary<string, object> where)
        {
            var result = false;
            if (await source.AnyAsync())
            {
                IQueryable<TEntity> exp;

                var whereCriteria = string.Empty;

                int i = 1;
                foreach (var item in where)
                {
                    if (item.Value.GetType() == typeof(string) || item.Value.GetType() == typeof(Guid))
                    {
                        whereCriteria += item.Key + "=\"" + item.Value + "\"";
                    }
                    else if (item.Value.GetType() == typeof(DateTime) || item.Value.GetType() == typeof(DateTime?))
                    {
                        var dateValue = DateTime.Parse(item.Value.ToString());
                        var dateValueAdd = DateTime.Parse(item.Value.ToString()).AddDays(1);

                        whereCriteria += item.Key + " >= DateTime(" + dateValue.Year + ", " + dateValue.Month + ", " + dateValue.Day + ") and " + item.Key + " < DateTime(" + dateValueAdd.Year + ", " + dateValueAdd.Month + ", " + dateValueAdd.Day + ")";
                    }
                    else
                    {
                        whereCriteria += item.Key + "=" + item.Value;
                    }

                    if (i < where.Count)
                    {
                        whereCriteria += " and ";
                    }

                    i++;
                }

                exp = source.Where(whereCriteria);
                result = await exp.AnyAsync();
            }

            return result;
        }

        public static async Task<bool> IsExistDataWithKeyAsync<TEntity>(this IQueryable<TEntity> source, ExistWithKeyModel model)
        {
            var result = false;

            if (await source.AnyAsync())
            {
                IQueryable<TEntity> oldData;

                if (model.KeyValue.GetType() == typeof(string) || model.KeyValue.GetType() == typeof(Guid))
                {
                    oldData = source.Where(model.KeyName + "=\"" + model.KeyValue + "\"");
                }
                else
                {
                    oldData = source.Where(model.KeyName + "=" + model.KeyValue);
                }

                var resultOldData = oldData.Select(model.FieldName).ToDynamicList();
                var oldValue = resultOldData[0];

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
                        else if (item.Value.GetType() == typeof(DateTime) || item.Value.GetType() == typeof(DateTime?))
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

                    exp = source.Where(whereCriteria);
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
                            else if (item.Value.GetType() == typeof(DateTime) || item.Value.GetType() == typeof(DateTime?))
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

                        exp = source.Where(whereCriteria);
                        result = await exp.AnyAsync();
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}
