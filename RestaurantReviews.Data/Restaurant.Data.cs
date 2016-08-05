using System.Data.Entity;

namespace RestaurantReviews.Data {
    internal partial class RestaurantReviewsEntities : DbContext {
        public RestaurantReviewsEntities()
            : base("name=RestaurantDataEntities") {
        }

        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<APIRequest> APIRequests { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<APIRequest>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<APIRequest>()
                .Property(e => e.PayloadDetails)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Restaurant>()
                .Property(e => e.WebsiteURL)
                .IsUnicode(false);

            modelBuilder.Entity<Review>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);
        }
    }
}
