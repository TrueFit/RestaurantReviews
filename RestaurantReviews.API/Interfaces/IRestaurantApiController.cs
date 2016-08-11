using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RestaurantReviews.Data;

namespace RestaurantReviews.API.Interfaces {
    
    /// <summary>
    /// An interface which provides the promise for basic functionality on our public-facing API.
    /// </summary>
    /// <typeparam name="T">Type of the object for which we are manipulating</typeparam>
    public interface IRestaurantApiController<T> {
        IHttpActionResult Create(T Data, int UserID);
        IHttpActionResult GetByID(int id);
    }
}