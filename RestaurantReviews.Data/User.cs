namespace RestaurantReviews.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        public bool Locked { get; set; }

        /// <summary>
        /// Create a new user in the db
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Locked"></param>
        /// <returns></returns>
        public static User AddUser(User NewUser) {
            using (var db = new RestaurantReviewsEntities()) {
                db.Users.Add(NewUser);
                return NewUser;
            }
        }
    }
}
