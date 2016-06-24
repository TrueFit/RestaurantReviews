using System;

namespace Truefit.Users.Models
{
    public class UserModel
    {
        public Guid Guid { get; set; }
        public string UserName { get; set; }
        public string AuthToken { get; set; }
    }
}
