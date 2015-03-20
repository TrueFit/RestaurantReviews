using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace RestaurantReviews.DAL
{
	/// <summary>
	/// Dummy repository to just return some simulated results
	/// </summary>
	public class ReviewRepository : IReviewRepository
	{
		public Review Get(int id)
		{
			Review review = new Review
			{
				Id = id,
				Author = "John Smith",
				Restaurant = "Burger Hut",
				ReviewText = "This was good!"
			};

			return review;
		}

		public IEnumerable<Review> GetByUser(string user)
		{
			List<Review> reviewsByUser = new List<Review>
			{
				new Review
				{
					Id = 323,
					Author = user,
					Restaurant = "Burger Hut",
					ReviewText = "This was good!"
				},
				new Review
				{
					Id = 555,
					Author = user,
					Restaurant = "Bravo!",
					ReviewText = "Bravo! (That's my review!)"
				}
			};

			return reviewsByUser;
		}

		public void Save(Review review)
		{
			review.Id = 232;
		}

		public void Delete(int id)
		{

		}
	}
}