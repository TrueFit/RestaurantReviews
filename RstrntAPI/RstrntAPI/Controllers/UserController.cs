using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RstrntAPI.Business;
using RstrntAPI.Business.Repositories;
using RstrntAPI.DTO;
using RstrntAPI.Models;

namespace RstrntAPI.Controllers
{
    [RoutePrefix("api/v1/User")]
    public class UserController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public List<UserDTO> GetAll()
        {
            return RepoRegistry.Get<IUserRepository>().GetAll();
        }

        [HttpGet()]
        [Route("{userId:int}"), Route("")]
        public UserDTO Get(int userId)
        {
            return RepoRegistry.Get<IUserRepository>().Get(userId);
        }

        [HttpGet()]
        [Route("{userId:int}/Reviews")]
        public UserReviews GetReviews(int userId)
        {
            var user = RepoRegistry.Get<IUserRepository>().Get(userId);
            var reviews = RepoRegistry.Get<IReviewRepository>().ListByUser(userId);

            return new UserReviews
            {
                User = user,
                Reviews = reviews
            };
        }

        [HttpPost()]
        [Route("")]
        public UserDTO Create(UserDTO user)
        {
            return RepoRegistry.Get<IUserRepository>().Create(user);
        }

        [HttpDelete()]
        [Route("{userId:int}"), Route("")]
        public bool Delete(int userId)
        {
            return RepoRegistry.Get<IUserRepository>().Delete(userId);
        }

        [HttpPut()]
        [Route("")]
        public UserDTO Update(UserDTO user)
        {
            return RepoRegistry.Get<IUserRepository>().Update(user);
        }
    }
}
