using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class GroupTask : BaseEntity<Guid>
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
    }
}
