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
using RestaurantReview.Models.TagModels;
using RestaurantReview.Utilities;
using AutoMapper;

namespace RestaurantReview.Controllers
{
    public class TagController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();

        // GET api/Tag
        [HttpGet]
        public List<string> GetTags(int restaurantId)
        {
            return db.Tags.Where(t => t.RestaurantId == restaurantId).Select(t => t.TagName).ToList();
        }

        // POST api/Tag
        [ResponseType(typeof(DisplayTagModel))]
        [AuthorizeMembership]
        [HttpPost]
        public IHttpActionResult PostTag(CreateDeleteTagModel tagModel)
        {
            string username = SessionUtilities.GetUserName(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (tagModel == null || db.Restaurants.Find(tagModel.RestaurantId) == null ||
                (db.Restaurants.Count(r => r.Id == tagModel.RestaurantId && r.OwnerUserName.Equals(username)) <= 0) ||
                (db.Tags.Count(t => t.TagName == tagModel.TagName && t.RestaurantId == tagModel.RestaurantId) > 0))
            {
                return BadRequest();
            }
            Tag tag = Mapper.Map<Tag>(tagModel);

            db.Tags.Add(tag);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Unable to add tag to this restaurant");
            }
            return CreatedAtRoute("DefaultApi", new { id = tag.Id }, Mapper.Map<DisplayTagModel>(tag));
        }

        // DELETE api/Tag/5
        [ResponseType(typeof(DisplayTagModel))]
        [AuthorizeMembership]
        [HttpPost]
        [Route("api/tag/delete")]
        public IHttpActionResult DeleteTag(CreateDeleteTagModel tagModel)
        {
            DisplayTagModel returnModel = new DisplayTagModel();
            // Validate user input
            string username = SessionUtilities.GetUserName(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Tag tag = db.Tags.Where(t => t.TagName == tagModel.TagName && t.RestaurantId == tagModel.RestaurantId).FirstOrDefault();
            if (tag == null || db.Restaurants.Count(r => r.Id == tag.RestaurantId && r.OwnerUserName.Equals(username)) <= 0)
            {
                return NotFound();
            }

            // Delete the tag from the database
            returnModel = Mapper.Map<DisplayTagModel>(tag);
            db.Tags.Remove(tag);
            db.SaveChanges();

            return Ok(returnModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return db.Tags.Count(e => e.Id == id) > 0;
        }
    }
}