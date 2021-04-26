using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class GroupTaskSpecification : SpecificationQuery<GroupTask>
    {
        public GroupTaskSpecification()
        {
            ApplyOrderBy(c => c.Name);
        }

        public GroupTaskSpecification(bool isActive) : base(c => c.IsActive == isActive)
        {
            ApplyOrderBy(c => c.Name);
        }
    }
}
