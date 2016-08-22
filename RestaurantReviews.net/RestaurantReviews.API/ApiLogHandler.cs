using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using RestaurantReviews.Data;

namespace RestaurantReviews.API {

    /// <summary>
    /// This class will handle the API access logging.
    /// </summary>
    public class ApiLogHandler : DelegatingHandler {

        /// <summary>
        /// OVERRIDE: Send an HTTP request to the inner handler to send to the server as an asynchronous operation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An HTTP response</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null) {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task => {
                        apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task => {
                    var response = task.Result;

                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    apiLogEntry.ResponseTimeStamp = DateTime.Now;

                    if (response.Content != null) {
                        apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }

                    ApiLogEntry.AddApiLogEntry(apiLogEntry);

                    return response;
                }, cancellationToken);
        }

        /// <summary>
        /// Create an API Log entry that includes request data
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The newly-created log entry</returns>
        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request) {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();

            return new ApiLogEntry() {
                Application = "RestaurantReviews.API",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                RequestRouteData = null, //SerializeRouteData(routeData),
                RequestIPAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimeStamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        /// <summary>
        /// Serialize the Web API 2 route date.
        /// TODO: Solve the issue with the JSON serializer. The IHttpRouteData object contains a circular referene that the serializer
        /// can't handle. Thusly, this method is not implemented at this time and will return a new NotImplementedException();
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns>new NotImplementedException()</returns>
        private string SerializeRouteData(IHttpRouteData routeData) {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers) {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList()) {
                if (item.Value != null) {
                    var header = String.Empty;
                    foreach (var value in item.Value) {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}