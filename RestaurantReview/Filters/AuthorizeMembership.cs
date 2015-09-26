using RestaurantReview.Models;
using RestaurantReview.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                string clientAuthToken = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
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
                        actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    }
                    else
                    {
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