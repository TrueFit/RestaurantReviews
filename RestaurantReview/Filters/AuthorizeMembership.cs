using RestaurantReview.Models;
using RestaurantReview.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Security;


namespace RestaurantReview.Filters
{
    public class AuthorizeMembership : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                // Parse out the token from the authorization header
                Regex r = new Regex("^"+SessionUtilities.TokenType+" ");
                string authHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                string clientAuthToken = authHeader == null ? null : r.Replace(authHeader, "");
                
                if (clientAuthToken != null)
                {
                    // Check the Sessions table to see if the user is logged in
                    Session session = (
                        from sessions in SessionUtilities.db.Sessions
                        where sessions.AuthToken.Equals(clientAuthToken)
                        && sessions.EndTime > DateTime.Now
                        select sessions
                    ).FirstOrDefault();


                    if (session == null)
                    {
                        // Stop any further progression
                        actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        // Add the user's username to the route data
                        actionContext.ControllerContext.RouteData.Values.Add("MemberUserName", session.UserName);
                        SessionUtilities.RefreshSession(session.UserName);
                    }
                }
                else
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                }
            }
            catch
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}