using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Business.Repositories;
using RstrntAPI.DTO;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/Reviews")]
    public class ReviewsController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public List<ReviewDTO> GetAll()
        {
            return RepoRegistry.Get<IReviewRepository>().GetAll();
        }

        [HttpGet()]
        [Route("{reviewId:int}"), Route("")]
        public ReviewDTO Get(int reviewId)
        {
            return RepoRegistry.Get<IReviewRepository>().Get(reviewId);
        }

        [HttpPost()]
        [Route("")]
        public ReviewDTO Create(ReviewDTO review)
        {
            return RepoRegistry.Get<IReviewRepository>().Create(review);
        }

        [HttpDelete()]
        [Route("{reviewId:int}"), Route("")]
        public bool Delete(int reviewId)
        {
            return RepoRegistry.Get<IReviewRepository>().Delete(reviewId);
        }

        [HttpPut()]
        [Route("")]
        public ReviewDTO Update(ReviewDTO review)
        {
            return RepoRegistry.Get<IReviewRepository>().Update(review);
        }
    }
}
