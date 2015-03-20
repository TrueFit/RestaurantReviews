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
	public class RestaurantRepository : IRestaurantRepository
	{
		public IEnumerable<Restaurant> GetByCity(string cityName)
		{
			var restaurants = new List<Restaurant>()
			{
				new Restaurant
				{
					Id = 23,
					Name = "Ruth's Cris",
					City = "Pittsburgh"
				},
				new Restaurant
				{
					Id = 243,
					Name = "Domino's",
					City = "Pittsburgh"
				}
			};

			return restaurants;
		}

		public void Save(Restaurant restaurant)
		{

		}
	}
}