using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Interfaces {
    public interface IRestaurantApiController<T> {
        IHttpActionResult Create(T Data, int UserID);
        IHttpActionResult GetByID(int id);
    }
}