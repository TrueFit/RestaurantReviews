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
        public IHttpActionResult Register(CreateUserModel user)
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

        // POST /api/user/login
        [ResponseType(typeof(UserLoginResponseModel))]
        public IHttpActionResult Login(LoginUserModel user)
        {
            UserLoginResponseModel returnModel = new UserLoginResponseModel();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (user.grant_type.ToLower() != "password")
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
        public IHttpActionResult Logout(LogoutUserModel user)
        {
            try
            {
                SessionUtilities.RemoveSession(user.UserName);
            }
            catch
            {
                return InternalServerError();
            }

            return Ok("Logout successful");
        }
    }
}
