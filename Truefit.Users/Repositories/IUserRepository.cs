using System;
using System.Threading.Tasks;
using Truefit.Users.Models;

namespace Truefit.Users.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetByGuid(Guid guid);
        Task<UserModel> GetByFacebookId(int id);
        Task Insert(UserModel user);
        Task Update(UserModel user);
    }
}
