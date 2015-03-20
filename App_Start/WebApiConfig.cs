using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RestaurantReviews
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "RestaurantsByCity",
				routeTemplate: "api/restaurant/city/{city}",
				defaults: new { controller = "Restaurant" }
			);

			config.Routes.MapHttpRoute(
				name: "Restaurant",
				routeTemplate: "api/restaurant/name/{restaurantName}",
				defaults: new { controller = "Restaurant" }
			);

			config.Routes.MapHttpRoute(
				name: "ReviewByRestaurant",
				routeTemplate: "api/restaurant/{restaurantName}/review",
				defaults: new { controller = "Review" }
			);

			config.Routes.MapHttpRoute(
				name: "ReviewByUser",
				routeTemplate: "api/review/user/{user}",
				defaults: new { controller = "Review" }
			);

			config.Routes.MapHttpRoute(
				name: "ReviewById",
				routeTemplate: "api/review/{Id}",
				defaults: new { controller = "Review" }
			);
        }
    }
}
