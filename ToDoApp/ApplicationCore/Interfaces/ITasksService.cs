using ApplicationCore.Entities;
using ApplicationCore.Helpers.BaseEntity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITasksService
    {
        Task<IReadOnlyList<Tasks>> GetAllAsync();

        Task<IReadOnlyList<Tasks>> GetAllAsync(Guid fkGroupTaskId);

        Task<Tasks> GetAsync(Guid id);

        Task<Guid?> PostAsync(Tasks model);

        Task<Guid?> PutAsync(Tasks model);

        Task<Guid?> PutCompletedTaskAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<bool> IsExistDataAsync(IDictionary<string, object> where);

        Task<bool> IsExistDataWithKeyAsync(ExistWithKeyModel model);
    }
}
