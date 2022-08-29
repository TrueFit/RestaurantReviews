namespace NoREST.Models.Interfaces
{
    public interface IOwnedEntity<T, TId> where T : IEntity<TId>, IPermissioned
    {
        T CreatedBy { get; set; }
        TId CreatedById { get; set; }
    }
}
