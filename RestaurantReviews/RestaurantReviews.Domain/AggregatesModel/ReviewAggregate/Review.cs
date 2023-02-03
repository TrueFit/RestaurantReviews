using RestaurantReviews.Domain.Seedwork;

namespace RestaurantReviews.Domain.AggregatesModel.ReviewAggregate
{
    public class Review : Entity, IAggregateRoot
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public Review(int restaurantId, int userId, string text, int rating)
        {
            RestaurantId = restaurantId;
            UserId = userId;
            Text = text;
            Rating = rating;
            Active = true;

            // Audit info
            var createdOn = DateTime.Now;
            CreatedOn = createdOn;
            LastUpdatedOn = createdOn;
        }

        public void Update(Review review) {
            Text = review.Text;
            Rating = review.Rating;
            LastUpdatedOn = DateTime.Now;
        }

        public void Delete()
        {
            Active = false;
            LastUpdatedOn = DateTime.Now;
        }
    }
}
