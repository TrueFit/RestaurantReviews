using RstrntAPI.DTO;

namespace RstrntAPI.Models
{
    public class UserReviewResponse
    {
        public UserDTO User { get; set; }
        public ReviewDTO Review { get; set; }
    }
}
