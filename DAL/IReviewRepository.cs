using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.DAL
{
	public interface IReviewRepository
	{
		Review Get(int id);
		IEnumerable<Review> GetByUser(string user);
		void Save(Review review);
		void Delete(int id);
	}
}
