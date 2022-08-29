namespace NoREST.Models.Interfaces
{
    public interface IAuditEntity
    {
        DateTime CreatedOn { get; set; }
        DateTime LastModifiedOn { get; set; }
    }
}
