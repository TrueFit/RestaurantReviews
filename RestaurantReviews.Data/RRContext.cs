using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantReviews.Core;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviews.Data
{
    public class RRContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantReview> RestaurantReviews { get; set; }

        public RRContext():base("Data Source=localhost;Initial Catalog=RestaurantReviewData;Integrated Security=true;")
        {
            //turn off lazy loading. I like eager loading for more control
            this.Configuration.LazyLoadingEnabled = false;

            //"Data Source=pitmainltp16;Initial Catalog=RestaurantReview2;User ID=sa;Password=mcse70290!"
            //this is a hack to ensure that entity framework works in other projects where this project is referenced. This makes it so
            //that the other projects do not need to include the entity framework package within.
            var ensureDLL = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        /*
         * I like to define property constraints and foreign keys within the context as it keeps the models clean of any 
         * ORM specific attributes. Keeps the model classes basic in case the Data layer ever needs changed to a different
         * ORM like Dapper, NHibernate, Rollw your own, etc.
         */
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //added index columns to both Name and City since these are the key columns to be search on
            modelBuilder.Entity<Restaurant>().Property(r => r.RestaurantName).HasMaxLength(200).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                     new IndexAnnotation(new IndexAttribute("IX_RestaurantName", 1)));
            modelBuilder.Entity<Restaurant>().Property(r => r.City).HasMaxLength(100).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                     new IndexAnnotation(new IndexAttribute("IX_RestaurantCity", 1)));

            modelBuilder.Entity<Restaurant>().Property(r => r.Description).HasColumnType("text");
            modelBuilder.Entity<Restaurant>().Property(r => r.FoodType).HasMaxLength(100);
            modelBuilder.Entity<Restaurant>().Property(r => r.Street).HasColumnType("text");
            modelBuilder.Entity<Restaurant>().Property(r => r.State).HasMaxLength(2).IsRequired();
            modelBuilder.Entity<Restaurant>().Property(r => r.PostalCode).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<Restaurant>().Property(r => r.CreatorId).IsRequired();
            modelBuilder.Entity<Restaurant>().HasRequired(r => r.Creator).WithMany(b => b.CreatedRestaurants).HasForeignKey(r => r.CreatorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<RestaurantReview>().Property(r => r.Comments).HasColumnType("text");

            //setup foreign key relationship between Restaurant and RestaurantReviews
            modelBuilder.Entity<RestaurantReview>().HasRequired(r => r.Restaurant).WithMany(b => b.Reviews).HasForeignKey(r => r.RestaurantId);
            modelBuilder.Entity<RestaurantReview>().HasRequired(r => r.Creator).WithMany(b => b.UserReviews).HasForeignKey(r => r.CreatorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>().Property(u => u.FirstName).HasMaxLength(75).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(75).IsRequired();

            //sets up the email property with a unique index since the email will act as a username
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(200).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, 
                                     new IndexAnnotation(new IndexAttribute("IX_UserEmail", 1) { IsUnique = true }));

        }
    }
}
