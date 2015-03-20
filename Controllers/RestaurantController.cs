using AutoMapper;
using RestaurantReviews.DAL;
using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RestaurantReviews.Controllers
{
    public class RestaurantController : ApiController
    {
		IRestaurantRepository _restaurantRepository;

		public RestaurantController(IRestaurantRepository restaurantRepository)
		{
			_restaurantRepository = restaurantRepository;
		}

		public IEnumerable<RestaurantModel> Get(string city)
		{
			IEnumerable<Restaurant> restaurants = _restaurantRepository.GetByCity(city);

			if (restaurants != null && restaurants.Count() == 0)
				return null;

			ModelFactory modelFactory = new ModelFactory();

			List<RestaurantModel> restaurantModels = new List<RestaurantModel>();

			foreach (var restaurant in restaurants)
			{
				var restaurantModel = modelFactory.Create(Request, restaurant);
				restaurantModels.Add(restaurantModel);
			}

			return restaurantModels;
		}

		public void Post()
		{

		}

		public HttpResponseMessage Put(string restaurantName, [FromBody] RestaurantModel restaurantModel)
		{
			if (restaurantModel == null)
				return Request.CreateResponse(HttpStatusCode.NotFound);

			try
			{
				restaurantModel.Name = restaurantName;

				var restaurant = Mapper.Map<Restaurant>(restaurantModel);

				_restaurantRepository.Save(restaurant);

				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
    }
}