using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace RestaurantReviews.Models
{
	public class ModelFactory
	{
		public RestaurantModel Create(HttpRequestMessage request, Restaurant restaurant)
		{
			RestaurantModel restaurantModel = Mapper.Map<RestaurantModel>(restaurant);
			
			return restaurantModel;
		}
	}
}