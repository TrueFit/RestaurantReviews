using RestaurantReview.Models;
using RestaurantReview.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using AutoMapper;

namespace RestaurantReview.Controllers
{
    public class UserController : ApiController
    {
        

        // POST /api/user
        [ResponseType(typeof(SafeUserModel))]
        public IHttpActionResult RegisterUser(CreateUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Membership.CreateUser(user.UserName, user.Password, user.Email);
            }
            catch (MembershipCreateUserException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Unable to create user");
            }

            return Ok<SafeUserModel>(Mapper.Map<SafeUserModel>(user));
        }
    }
}
