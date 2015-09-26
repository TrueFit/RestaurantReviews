using System;
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

namespace RestaurantReview.Controllers
{
    public class ReviewController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();

        // GET api/Review
        public IQueryable<Review> GetReviews()
        {
            return db.Reviews;
        }

        // GET api/Review/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult GetReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // PUT api/Review/5
        public IHttpActionResult PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.Id)
            {
                return BadRequest();
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Review
        [ResponseType(typeof(Review))]
        [AuthorizeMembership]
        public IHttpActionResult PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.UserName = GetUserName(Request);
            review.Timestamp = DateTime.Now;
            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = review.Id }, review);
        }

        // DELETE api/Review/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return Ok(review);
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
    }
}