using NoREST.Models.Interfaces;

namespace NoREST.Domain
{
    public class AuditLogic : IAuditLogic
    {
        public void SetAuditValues(IAuditEntity auditEntity, bool isNew = false, DateTime? now = null)
        {
            DateTime hereNow = now ?? DateTime.UtcNow;

            if (isNew) auditEntity.CreatedOn = hereNow;

            auditEntity.LastModifiedOn = hereNow;
        }

        public void SetAuditAndOwnershipValues<T, TOwner, TId>(T entity, TOwner owner, bool isNew = false, DateTime? now = null)
            where T : IAuditEntity, IOwnedEntity<TOwner, TId>
            where TOwner : IEntity<TId>, IPermissioned
        {
            SetAuditValues(entity, isNew, now);

            entity.CreatedById = owner.Id;
        }
    }
}