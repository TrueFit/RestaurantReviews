using System.Threading.Tasks;
using Truefit.Users.Models;

namespace Truefit.Users
{
    //Todo: Create a real user account service
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user by AuthToken
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        Task<UserModel> Authenticate(string authToken);
    }
}
