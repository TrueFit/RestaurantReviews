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

            modelBuilder.Entity<User>()
                .HasData(
                     new User { Id = 1, IdentityProviderId = "5qg4one8ve2o7024st8dks8ejq", UserName = "NoRESTApi", IsAdmin = true, IsSystemUser = true },
                     new User { Id = 2, IdentityProviderId = "b9c45965-95d5-4d45-a580-bafeba254ad0", UserName = "chris.rune", IsAdmin = true, IsSystemUser = false }
                );

        }
    }
}