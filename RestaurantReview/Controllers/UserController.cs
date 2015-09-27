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
using RestaurantReview.Utilities;
using System.Diagnostics;
using RestaurantReview.Models.ResponseModels;
using System.Web.Script.Serialization;
using RestaurantReview.Filters;

namespace RestaurantReview.Controllers
{
    public class UserController : ApiController
    {
        // POST /api/user/register
        [ResponseType(typeof(SafeUserModel))]
        [Route("api/user/register")]
        [HttpPost]
        public IHttpActionResult Register(CreateUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user == null)
            {
                return BadRequest();
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
                return BadRequest();
            }

            return Ok<SafeUserModel>(Mapper.Map<SafeUserModel>(user));
        }

        // POST /api/user/login
        [ResponseType(typeof(UserLoginResponseModel))]
        [Route("api/user/login")]
        [HttpPost]
        public IHttpActionResult Login(LoginUserModel user)
        {
            UserLoginResponseModel returnModel = new UserLoginResponseModel();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (user == null || user.grant_type.ToLower() != "password")
            {
                return BadRequest(new JavaScriptSerializer().Serialize(ErrorModels.InvalidGrant));
            }
            if (Membership.ValidateUser(user.UserName, user.Password))
            {
                try
                {
                    returnModel = SessionUtilities.CreateSession(user.UserName);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
                return Ok<UserLoginResponseModel>(returnModel);
            }
            else
            {
                return BadRequest( new JavaScriptSerializer().Serialize(ErrorModels.AccessDenied) );
            }
        }

        // POST /api/user/logout
        [AuthorizeMembership]
        [Route("api/user/logout")]
        [HttpPost]
        public IHttpActionResult Logout(LogoutUserModel user)
        {
            if (!ModelState.IsValid || user == null)
            {
                return BadRequest("UserName must be provided");
            }

            try
            {
                SessionUtilities.RemoveSession(user.UserName);
            }
            catch
            {
                return BadRequest();
            }

            return Ok("Logout successful");
        }
    }
}
