using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace RestaurantReviews.Data {
    /// <summary>
    /// RestaurantReviews DB Data context
    /// </summary>
    internal partial class RestaurantReviewsEntities : DbContext {
        public RestaurantReviewsEntities()
            : base("name=RestaurantDataEntities") {
        }

        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ApiLogEntry> ApiLogEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
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
                .Property(e => e.State)
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

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.Application)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.User)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.Machine)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestIPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestContentType)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestContentBody)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestUri)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestMethod)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestRouteTemplate)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestRouteData)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.RequestHeaders)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.ResponseContentType)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.ResponseContentBody)
                .IsUnicode(false);

            modelBuilder.Entity<ApiLogEntry>()
                .Property(e => e.ResponseHeaders)
                .IsUnicode(false);
        }

        public override int SaveChanges() {
            try {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex) {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
