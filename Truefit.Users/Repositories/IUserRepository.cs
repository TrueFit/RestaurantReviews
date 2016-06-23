using System;
using System.Collections.Generic;
using Truefit.Users.Models;

namespace Truefit.Users.Repositories
{
    public interface IUserRepository
    {
        UserModel GetByGuid(Guid guid);
        IEnumerable<UserModel> GetByFacebookId(int id);
    }
}
