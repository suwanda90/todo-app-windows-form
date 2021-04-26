using System;
using ApplicationCore.Interfaces.BaseEntity;

namespace ApplicationCore.Entities
{
    public class BaseEntity<TId> : IModel<TId>, IAuditableEntity
    {
        public virtual TId Id { get;  set; }

        public DateTime? DateCreated { get;  set; }

        public string CreatedBy { get; set; }

        public DateTime? DateModified { get;  set; }

        public string ModifiedBy { get;  set; }

        public bool IsActive { get;  set; }
    }

    public interface IAuditableEntity
    {
        DateTime? DateCreated { get;  set; }

        DateTime? DateModified { get;  set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
