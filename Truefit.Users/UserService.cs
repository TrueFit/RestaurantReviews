using System;
using System.Threading.Tasks;
using Truefit.Users.Models;

namespace Truefit.Users
{
    public class UserService : IUserService
    {
        /*
         * Warning: This account service is temporary, calling this method only returns a random user.
         * */
        public async Task<UserModel> Authenticate(string authToken)
        {
            return new UserModel
            {
                Guid = Guid.NewGuid(),
                UserName = "Test User",
                AuthToken = authToken
            };
        }
    }
}
