using System;
using System.Threading.Tasks;
using Truefit.Users.Models;

namespace Truefit.Users.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a User by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<UserModel> GetByGuid(Guid guid);

        /// <summary>
        /// Gets a User by Facebook Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserModel> GetByFacebookId(int id);

        /// <summary>
        /// Inserts a User into the Repository
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Insert(UserModel user);

        /// <summary>
        /// Updates a User into the Repository
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Update(UserModel user);
    }
}
