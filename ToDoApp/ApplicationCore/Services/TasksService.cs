using ApplicationCore.Entities;
using ApplicationCore.Helpers.BaseEntity.Model;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.BaseEntity;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class TasksService : ITasksService
    {
        private readonly IEfRepository<Tasks, Guid> _repository;

        public TasksService(IEfRepository<Tasks, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Tasks>> GetAllAsync()
        {
            var spec = new TasksSpecification();
            return await _repository.GetAllAsync(spec);
        }

        public async Task<IReadOnlyList<Tasks>> GetAllAsync(Guid FkGroupTaskId)
        {
            var spec = new TasksSpecification();
            var data = await _repository.GetAllAsync(spec);

            return data.Where(x => x.FkGroupTaskId == FkGroupTaskId).ToList();
        }

        public async Task<Tasks> GetAsync(Guid id)
        {
            var spec = new TasksSpecification();
            return await _repository.GetAsync(spec, id);
        }

        public async Task<Guid?> PostAsync(Tasks model)
        {
            Guid? id = null;

            var where = new Dictionary<string, object>
            {
                { nameof(model.Name), model.Name}
            };

            if (model.FkGroupTaskId.HasValue)
            {
                where.Add(nameof(model.FkGroupTaskId), model.FkGroupTaskId);
            }

            if (!await _repository.IsExistDataAsync(where))
            {
                var data = await _repository.AddAsync(model);
                id = data.Id;
            }

            return id;
        }

        public async Task<Guid?> PutAsync(Tasks model)
        {
            Guid? id = null;

            var where = new Dictionary<string, object>
            {
                { nameof(model.Name), model.Name}
            };

            if (model.FkGroupTaskId.HasValue)
            {
                where.Add(nameof(model.FkGroupTaskId), model.FkGroupTaskId);
            }

            var param = new ExistWithKeyModel
            {
                KeyName = nameof(model.Id),
                KeyValue = model.Id,
                FieldName = nameof(model.Name),
                FieldValue = model.Name,
                WhereData = where
            };

            if (!await _repository.IsExistDataWithKeyAsync(param))
            {
                await _repository.UpdateAsync(model);
                id = model.Id;
            }

            return id;
        }

        public async Task<Guid?> PutCompletedTaskAsync(Guid id)
        {
            Guid? fkTaskId = null;
            var model = await GetAsync(id);

            if (model != null)
            {
                model.IsCompleted = true;

                await _repository.UpdateAsync(model);
                fkTaskId = model.Id;
            }

            return fkTaskId;
        }

        public async Task DeleteAsync(Guid id)
        {
            var data = await _repository.GetAsync(id);
            await _repository.DeleteAsync(data);
        }

        public async Task<bool> IsExistDataAsync(IDictionary<string, object> where)
        {
            return await _repository.IsExistDataAsync(where);
        }

        public async Task<bool> IsExistDataWithKeyAsync(ExistWithKeyModel model)
        {
            return await _repository.IsExistDataWithKeyAsync(model);
        }
    }
}
