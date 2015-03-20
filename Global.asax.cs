using AutoMapper;
using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RestaurantReviews
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			AutoMapperSetup();
		}

		private void AutoMapperSetup()
		{
			Mapper.CreateMap<Restaurant, RestaurantModel>();
			Mapper.CreateMap<Review, ReviewModel>();

			Mapper.CreateMap<RestaurantModel, Restaurant>();
			Mapper.CreateMap<ReviewModel, Review>();
		}
	}
}
