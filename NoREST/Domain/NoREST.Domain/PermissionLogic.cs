using NoREST.Models.Interfaces;

namespace NoREST.Domain
{
    public interface IPermissionLogic
    {
        bool CanModify<T>(IOwnedEntity<T, int> ownedEntity, T requester) where T : IEntity<int>, IPermissioned;
    }

    public class PermissionLogic : IPermissionLogic
    {
        public bool CanModify<T>(IOwnedEntity<T, int> ownedEntity, T requester) where T : IEntity<int>, IPermissioned
        {
            return requester.IsAdmin || ownedEntity.CreatedById == requester.Id;
        }
    }
}
