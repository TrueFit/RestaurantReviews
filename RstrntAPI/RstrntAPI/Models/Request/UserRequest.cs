using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Models.Request
{
    public class UserRequest
    {
        public int? Id { get; set; }
        public string AccountName { get; set; }
        public string FullName { get; set; }
        public int? Hometown { get; set; }
    }
}
