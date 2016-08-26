namespace RestaurantReviews.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RestaurantReviews.Core;

    internal sealed class Configuration : DbMigrationsConfiguration<RestaurantReviews.Data.RRContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RestaurantReviews.Data.RRContext context)
        {

            User usr = context.Users.Where(u => u.Email == "admin@restaurantreviews.com").FirstOrDefault();
            if (usr == null)
            {
                usr = new User();
                usr.FirstName = "System";
                usr.LastName = "Administrator";
                usr.Email = "admin@restaurantreviews.com";
                context.Users.Add(usr);
                context.SaveChanges();
            }
            
        }
    }
}
