using ApplicationCore.Entities;
using ApplicationCore.Helpers.BaseEntity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IGroupTaskService
    {
        Task<IReadOnlyList<GroupTask>> GetAllAsync();

        Task<GroupTask> GetAsync(Guid id);

        Task<Guid?> PostAsync(GroupTask model);

        Task<Guid?> PutAsync(GroupTask model);

        Task DeleteAsync(Guid id);

        Task<bool> IsExistDataAsync(IDictionary<string, object> where);

        Task<bool> IsExistDataWithKeyAsync(ExistWithKeyModel model);
    }
}
