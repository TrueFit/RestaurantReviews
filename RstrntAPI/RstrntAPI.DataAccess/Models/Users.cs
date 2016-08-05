using Massive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DataAccess.Models
{
    public class Users : DynamicModel
    {
        public Users() : base("LocalDatabase", "users", "id", "", "id") { }
    }

    public class UsersEntity
    {
        public int? id { get; set; }
        public string acct_name { get; set; }
        public string full_name { get; set; }
        public int? hometown { get; set; }
    }
}
