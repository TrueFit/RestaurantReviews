using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.DAL
{
	public interface IRestaurantRepository
	{
		IEnumerable<Restaurant> GetByCity(string cityName);
		void Save(Restaurant restaurant);
	}
}
