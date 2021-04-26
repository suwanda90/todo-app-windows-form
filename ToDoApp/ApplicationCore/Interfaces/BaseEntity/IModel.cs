namespace ApplicationCore.Interfaces.BaseEntity
{
    public interface IModel { }

    public interface IModel<TId> : IModel
    {
        TId Id { get;  set; }
    }
}
