using System;
using Truefit.Users.Models;

namespace Truefit.Users.Repositories
{
    public interface IUserRepository
    {
        UserModel GetByGuid(Guid guid);
        UserModel GetByFacebookId(int id);
        void Insert(UserModel user);
        void Update(UserModel user);
    }
}
