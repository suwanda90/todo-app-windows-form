using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    public class Tasks : BaseEntity<Guid>
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [ForeignKey("FkGroupTaskId")]
        public GroupTask GroupTask { get; set; }
        public Guid? FkGroupTaskId { get; set; }

        public DateTime? DueDate { get; set; }


        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]

        public string Note { get; set; }

        public bool IsCompleted { get; set; }
    }
}
