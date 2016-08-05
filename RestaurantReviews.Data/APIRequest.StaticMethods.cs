using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RestaurantReviews.Data {

    public partial class APIRequest {
        public static void LogRequest(APIRequest Request) {
            using (var db = new RestaurantReviewsEntities()) {
                db.APIRequests.Add(Request);
                db.SaveChanges();
            }
        }

        public static List<APIRequest> GeAPIRequests(DateTime StartDate, DateTime EndDate) {
            using (var db = new RestaurantReviewsEntities()) {
                var ReturnList = db.APIRequests
                    .Where(r => r.AccessDateTime >= StartDate && r.AccessDateTime <= EndDate)
                    .OrderBy(r => r.AccessDateTime).ToList();

                return ReturnList;
            }
        }
    }
}
