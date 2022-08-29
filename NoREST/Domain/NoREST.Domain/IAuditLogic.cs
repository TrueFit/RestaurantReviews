using NoREST.Models.Interfaces;

namespace NoREST.Domain
{
    public interface IAuditLogic
    {
        void SetAuditAndOwnershipValues<T, TOwner, TId>(T entity, TOwner owner, bool isNew = false, DateTime? now = null)
            where T : IAuditEntity, IOwnedEntity<TOwner, TId>
            where TOwner : IEntity<TId>, IPermissioned;
        void SetAuditValues(IAuditEntity auditEntity, bool isNew = false, DateTime? now = null);
    }
}