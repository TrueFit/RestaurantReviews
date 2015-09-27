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
using RestaurantReview.Models.CommentModels;
using AutoMapper;
using RestaurantReview.Filters;
using RestaurantReview.Utilities;

namespace RestaurantReview.Controllers
{
    public class CommentController : ApiController
    {
        private RestRevEntities db = new RestRevEntities();

        // GET api/Comment
        // Search for comments based on SearchCommentModel properties
        [HttpGet]
        public List<DisplayCommentModel> GetComments([FromUri]SearchCommentModel commentModel)
        {
            IQueryable<Comment> filteredComments = db.Comments;
            List<DisplayCommentModel> displayComments = new List<DisplayCommentModel>();

            if (commentModel != null)
            {
                // Filter the comments
                if (commentModel.ReviewId > 0)
                {
                    filteredComments = filteredComments.Where(c => c.ReviewId == commentModel.ReviewId);
                }

                if (!String.IsNullOrWhiteSpace(commentModel.UserName))
                {
                    filteredComments = filteredComments.Where(c => c.UserName.Equals(commentModel.UserName));
                }

                // Order the comments by the orderby and order specified
                filteredComments = OrderComments(filteredComments, commentModel.OrderBy, commentModel.Order);

                // Get the requested page of comments based on the SearchModel's PageNum and NumReturned
                filteredComments = commentModel.GetPage(filteredComments);
            }

            foreach (Comment comment in filteredComments)
            {
                displayComments.Add(Mapper.Map<DisplayCommentModel>(comment));
            }

            return displayComments;
        }

        // GET api/Comment/5
        // Search for comment by id
        [ResponseType(typeof(DisplayCommentModel))]
        [HttpGet]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<DisplayCommentModel>(comment));
        }

        // PUT api/Comment/5
        // Allows a user to edit a comment that they authored
        [AuthorizeMembership]
        [HttpPut]
        public IHttpActionResult PutComment(int id, UpdateCommentModel commentModel)
        {
            string username = SessionUtilities.GetUserName(Request);

            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentModel == null || id != commentModel.Id || !CommentExists(id, username))
            {
                return BadRequest();
            }
            commentModel.UserName = username;

            // Update the comment
            Comment comment = db.Comments.Find(commentModel.Id);
            comment.Content = commentModel.Content;
            comment.Timestamp = DateTime.Now;
            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Unable to update comment");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Comment
        // Allows a user to post a comment to the Review given by the model's ReviewId
        [ResponseType(typeof(Comment))]
        [AuthorizeMembership]
        [HttpPost]
        public IHttpActionResult PostComment(CreateCommentModel commentModel)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (commentModel == null)
            {
                return BadRequest();
            }
            if (db.Reviews.Find(commentModel.ReviewId) == null)
            {
                return BadRequest("Specified review not found");
            }

            // Create new comment
            commentModel.UserName = SessionUtilities.GetUserName(Request);
            Comment comment = Mapper.Map<Comment>(commentModel);
            db.Comments.Add(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Failed to create comment");
            }

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, Mapper.Map<DisplayCommentModel>(comment));
        }

        // DELETE api/Comment/5
        [ResponseType(typeof(DisplayCommentModel))]
        [AuthorizeMembership]
        [HttpPost]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null || !CommentExists(id, SessionUtilities.GetUserName(Request)))
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(Mapper.Map<DisplayCommentModel>(comment));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }

        private bool CommentExists(int id, string username = null)
        {
            return username == null ?
                db.Comments.Count(c => c.Id == id) > 0 :
                db.Comments.Count(c => c.Id == id && c.UserName.Equals(username)) > 0;
        }

        /* Orders the filtered comments by the following fields
         * - ReviewId
         * - UserName
         * - Timestamp
         */
        private IQueryable<Comment> OrderComments(IQueryable<Comment> comments, string orderby, string order)
        {
            comments = comments.OrderBy(c => c.Id);
            if (!String.IsNullOrWhiteSpace(orderby))
            {
                if (orderby.ToLower() == "reviewid")
                {
                    comments = order != null && order.ToLower() == "desc" ?
                        comments.OrderByDescending(c => c.ReviewId) :
                        comments.OrderBy(c => c.ReviewId);
                }
                else if (orderby.ToLower() == "username")
                {
                    comments = order != null && order.ToLower() == "desc" ?
                        comments.OrderByDescending(c => c.UserName) :
                        comments.OrderBy(c => c.UserName);
                }
                else if (orderby.ToLower() == "timestamp")
                {
                    comments = order != null && order.ToLower() == "desc" ?
                        comments.OrderByDescending(c => c.Timestamp) :
                        comments.OrderBy(c => c.Timestamp);
                }
            }

            return comments;
        }
    }
}