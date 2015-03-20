using AutoMapper;
using RestaurantReviews.DAL;
using RestaurantReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestaurantReviews.Controllers
{
    public class ReviewController : ApiController
    {
		IReviewRepository _reviewRepository;

		public ReviewController(IReviewRepository reviewRepository)
		{
			_reviewRepository = reviewRepository;
		}

		public IEnumerable<ReviewModel> Get(string user)
		{
			//Just getting the basic idea down here

			var reviews = _reviewRepository.GetByUser(user);

			List<ReviewModel> reviewModels = new List<ReviewModel>();

			foreach (var review in reviews)
			{
				var reviewModel = Mapper.Map<ReviewModel>(review);
				reviewModels.Add(reviewModel);
			}

			return reviewModels;
		}

		public HttpResponseMessage Post(string restaurantName, [FromBody] ReviewModel reviewModel)
		{
			//Just getting the basic idea down here

			var review = Mapper.Map<Review>(reviewModel);

			_reviewRepository.Save(review);

			return Request.CreateResponse(HttpStatusCode.Created);
		}

		public HttpResponseMessage Delete(int id)
		{
			Review review = _reviewRepository.Get(id);
			if (review == null)
				return Request.CreateResponse(HttpStatusCode.NotFound);

			_reviewRepository.Delete(id);

			return Request.CreateResponse(HttpStatusCode.OK);
		}
    }
}
