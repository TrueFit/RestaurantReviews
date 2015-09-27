using RestaurantReview.Models;
using RestaurantReview.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AutoMapper;
using System.Net.Http;

namespace RestaurantReview.Utilities
{
    public static class SessionUtilities
    {
        public static SessionsEntities db = new SessionsEntities();
        private const int SESSION_LEN = 20; // in minutes
        public static string TokenType = "RestRevToken"; // review rfc 6749 for acceptable token types (this is not one of them)

        // Manages the sessions database for an authorized user
        // NOTE: This assumes a valid and authorized username
        public static UserLoginResponseModel CreateSession(string username)
        {
            // If there's a current entry in the Session table for this user, we'll just update it
            Session session = (
                from sessions in db.Sessions
                where sessions.UserName.Equals(username)
                select sessions
            ).FirstOrDefault();
            
            if(session == null)
            {
                // If the user hasn't logged in before, create a new session entry in the Session table
                session = new Session()
                {
                    UserName = username
                };
                db.Sessions.Add(session);
            }

            session.AuthToken = GenerateToken();
            session.CreateTime = DateTime.Now;
            session.EndTime = DateTime.Now.AddMinutes(SESSION_LEN);
            db.SaveChanges();

            return Mapper.Map<UserLoginResponseModel>(session);
        }

        // Automatically remove a user's session record from the Sessions table
        // Returns the number of records removed (should only be 1)
        // NOTE: The user with this username is assumed to be authorized
        public static int RemoveSession(string username)
        {
            IEnumerable<Session> sessions = (
                from session in db.Sessions
                where session.UserName.Equals(username)
                select session
            );

            foreach (Session session in sessions)
            {
                db.Sessions.Remove(session);
            }
            db.SaveChanges();

            return sessions.Count();
        }

        // If the user has a current session, it will reset the end time to SESSION_LEN minutes from now
        public static void RefreshSession(string username)
        {
            IEnumerable<Session> sessions = (
                from session in db.Sessions
                where session.UserName.Equals(username)
                    && session.EndTime > DateTime.Now
                select session
            );

            foreach (Session session in sessions)
            {
                session.EndTime = DateTime.Now.AddMinutes(SESSION_LEN);
            }
            db.SaveChanges();
        }

        // Get the username of the user that's been authorized for this request
        // NOTE: The "MemberUserName" value is set in the AuthorizeMembership filter
        public static string GetUserName(HttpRequestMessage request)
        {
            return request.GetRouteData().Values["MemberUserName"].ToString();
        }

        private static string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}