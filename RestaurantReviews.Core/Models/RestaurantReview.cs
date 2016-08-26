namespace RestaurantReviews.Core
{
    public class RestaurantReview: TrackableModel
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public bool WouldRecommend { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }

        public override void Validate()
        {
            if (RestaurantId == 0)
                ValidationErrors.Add("Review must have an associated Restaurant");

            if (Rating <= 0 || Rating > 5)
                ValidationErrors.Add("Rating must be a number between 1 and 5");

            if (CreatorId == 0)
                ValidationErrors.Add("Creator is required");
        }
    }
}
