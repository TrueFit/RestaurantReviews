using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees;

namespace RestaurantReviews.Data {
    public partial class ApiLogEntry {

        /// <summary>
        /// Log a new entry API event to the database log table.
        /// </summary>
        /// <param name="Entry"></param>
        /// <returns>The newly-created log entry</returns>
        public static ApiLogEntry AddApiLogEntry(ApiLogEntry Entry) {
            using (var db = new RestaurantReviewsEntities()) {
                db.ApiLogEntries.Add(Entry);
                db.SaveChanges();
                return Entry;
            }
        }
    }
}
