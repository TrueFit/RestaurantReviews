using System;
using System.Threading.Tasks;
using Truefit.Users.Models;

namespace Truefit.Users
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Returns a temporary user
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public async Task<UserModel> Authenticate(string authToken)
        {
            return new UserModel
            {
                Guid = Guid.Parse("19685BD6-1B72-4DE9-BCB5-413DAFBA5DD0"),
                UserName = "Test User",
                AuthToken = authToken
            };
        }
    }
}
