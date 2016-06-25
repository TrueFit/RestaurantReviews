using System.Threading.Tasks;
using NUnit.Framework;
using Truefit.Users.Models;

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

        [Test]
        public async Task Authenticate_Should_Return_User_Always()
        {
            var actual = await this._userService.Authenticate(string.Empty);
            Assert.IsInstanceOf<UserModel>(actual);
        }
    }
}
