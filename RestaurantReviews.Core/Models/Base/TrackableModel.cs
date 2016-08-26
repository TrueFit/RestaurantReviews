using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Core
{
    public abstract class TrackableModel: BaseModel
    {
        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
