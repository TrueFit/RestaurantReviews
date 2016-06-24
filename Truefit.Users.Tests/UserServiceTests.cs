using NUnit.Framework;

namespace Truefit.Users.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        [SetUp]
        public void Setup()
        {
            this._userService = new UserService();
        }
    }
}
