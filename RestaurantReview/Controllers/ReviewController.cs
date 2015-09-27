﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurantReview.Models;
using RestaurantReview.Filters;
using RestaurantReview.Models.CustomRestRevModels;
using RestaurantReview.Models.ReviewModels;
using AutoMapper;

namespace RestaurantReview.Controllers
{
    public class ReviewController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();

        // GET api/Review
        public IEnumerable<DisplayReviewModel> GetReviews([FromUri]SearchReviewModel reviewModel)
        {
            IQueryable<Review> filteredReviews = db.Reviews;
            List<DisplayReviewModel> displayReviews = new List<DisplayReviewModel>();

            // Filter the reviews
            if (!String.IsNullOrWhiteSpace(reviewModel.UserName))
            {
                filteredReviews = filteredReviews.Where(r => r.UserName.Contains(reviewModel.UserName));
            }

            if (reviewModel.RestaurantId > 0)
            {
                filteredReviews = filteredReviews.Where(r => r.RestaurantId == reviewModel.RestaurantId);
            }

            // Order the reviews by the OrderBy and Order specified
            filteredReviews = OrderReviews(filteredReviews, reviewModel.OrderBy, reviewModel.Order);

            // Get page of reviews by quantity requested and page number
            filteredReviews = reviewModel.GetPage(filteredReviews);

            foreach (Review rev in filteredReviews)
            {
                displayReviews.Add(Mapper.Map<DisplayReviewModel>(rev));
            }

            return displayReviews;
        }

        // GET api/Review/5
        [ResponseType(typeof(DisplayReviewModel))]
        public IHttpActionResult GetReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<DisplayReviewModel>(review));
        }

        // PUT api/Review/5
        // Updates an existing review
        [AuthorizeMembership]
        public IHttpActionResult PutReview(int id, UpdateReviewModel reviewModel)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            reviewModel.UserName = GetUserName(Request);
            if (id != reviewModel.Id || !ReviewExists(id, reviewModel.UserName))
            {
                return BadRequest();
            }

            // Update the review
            Review review = db.Reviews.Find(reviewModel.Id);
            review.Rating = reviewModel.Rating;
            review.Content = reviewModel.Content;
            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Unable to update review");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Review
        // Inserts a new Review for the restaurant with given RestaurantId by the authorized user
        [ResponseType(typeof(DisplayReviewModel))]
        [AuthorizeMembership]
        public IHttpActionResult PostReview(CreateReviewModel reviewModel)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Restaurants.Find(reviewModel.RestaurantId) == null)
            {
                return BadRequest("Specified restaurant not found");
            }

            // Create new review
            reviewModel.UserName = GetUserName(Request);
            Review review = Mapper.Map<Review>(reviewModel);
            db.Reviews.Add(review);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Unable to update review. A user can only submit one review per restaurant.");
            }
            return CreatedAtRoute("DefaultApi", new { id = review.Id }, Mapper.Map<DisplayReviewModel>(review));
        }

        // DELETE api/Review/5
        [ResponseType(typeof(DisplayReviewModel))]
        [AuthorizeMembership]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return Ok(Mapper.Map<DisplayReviewModel>(review));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id, string username = null)
        {
            return username == null ?
                db.Reviews.Count(e => e.Id == id) > 0 :
                db.Reviews.Count(e => e.Id == id && e.UserName.Equals(username)) > 0;
        }

        private string GetUserName(HttpRequestMessage request)
        {
            return request.GetRouteData().Values["MemberUserName"].ToString();
        }

        /* Orders the filtered reviews query by the following fields
         * - UserName
         * - Rating
         */
        private IQueryable<Review> OrderReviews(IQueryable<Review> reviews, string orderby, string order)
        {
            reviews = reviews.OrderBy(r => r.Id);
            if (!String.IsNullOrWhiteSpace(orderby))
            {
                if (orderby.ToLower() == "username")
                {
                    reviews = order != null && order.ToLower() == "desc" ?
                        reviews.OrderByDescending(r => r.UserName) :
                        reviews.OrderBy(r => r.UserName);
                }
                else if (orderby.ToLower() == "rating")
                {
                    reviews = order != null && order.ToLower() == "desc" ?
                        reviews.OrderByDescending(r => r.Rating) :
                        reviews.OrderBy(r => r.Rating);
                }
            }
            
            return reviews;
        }
    }
}