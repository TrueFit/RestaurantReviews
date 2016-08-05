using RstrntAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.Models
{
    public class UserReviews
    {
        public UserDTO User { get; set; }
        public List<ReviewDTO> Reviews { get; set; }
    }
}
