using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;

namespace NoREST.DataAccess
{
    public class NoRESTContext : DbContext
    {
        public NoRESTContext(DbContextOptions<NoRESTContext> options)
            : base(options) { }

        internal DbSet<User> Users { get; set; }
        internal DbSet<Restaurant> Restaurants { get; set; }
        internal DbSet<Review> Reviews { get; set; }
        internal DbSet<UserRestaurantBan> UserRestaurantBans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            //modelBuilder.Entity<Restaurant>()
            //    .HasOne(r => r.CreatedBy).WithMany(u => u.Restaurants)
            //    .IsRequired(false)
            //    .HasForeignKey(r => r.CreatedById)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.CreatedBy).WithMany(u => u.Reviews).HasPrincipalKey(u => u.Id)
            //    .IsRequired(false)
            //    .HasForeignKey(u => u.CreatedById)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.Restaurant).WithMany(r => r.Reviews).HasPrincipalKey(r => r.Id)
            //    .IsRequired(false)
            //    .HasForeignKey(r => r.RestaurantId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //modelBuilder.Entity<Review>()
            //    .HasKey(r => r.Id);

            //modelBuilder.Entity<User>()
            //    .HasKey(u => u.Id);

            modelBuilder.Entity<UserRestaurantBan>()
                .HasKey(nameof(UserRestaurantBan.RestaurantId), nameof(UserRestaurantBan.UserId));
            modelBuilder.Entity<UserRestaurantBan>()
                .HasOne(b => b.User).WithMany(u => u.BannedRestaurants)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<UserRestaurantBan>()
                .HasOne(b => b.Restaurant).WithMany(r => r.BannedUsers)
                .HasForeignKey(b => b.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // new User { Id = 1, IdentityProviderId = "5qg4one8ve2o7024st8dks8ejq", UserName = "NoRESTApi", IsAdmin = true, IsSystemUser = true },
            // new User { Id = 2, IdentityProviderId = "b9c45965-95d5-4d45-a580-bafeba254ad0", UserName = "chris.rune", IsAdmin = true, IsSystemUser = false }
        }
    }
}