using ApplicationCore.Entities;
using System;

namespace ApplicationCore.Specifications
{
    public class TasksSpecification : SpecificationQuery<Tasks>
    {
        public TasksSpecification()
            : base(c => c.GroupTask.IsActive == true || c.GroupTask == null)
        {
            ApplyOrderBy(c => c.Name);

            AddIncludes(query => query
            .Include(i => i.GroupTask));
        }

        public TasksSpecification(bool isActive)
            : base(c => c.IsActive == isActive)
        {
            ApplyOrderBy(c => c.Name);

            AddIncludes(query => query
            .Include(i => i.GroupTask));
        }

        public TasksSpecification(bool isActive, Guid fkGroupTaskId)
            : base(c => c.IsActive == isActive && c.GroupTask.IsActive == true && c.FkGroupTaskId == fkGroupTaskId)
        {
            ApplyOrderBy(c => c.Name);

            AddIncludes(query => query
            .Include(i => i.GroupTask));
        }
    }
}
