using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;

namespace NoREST.DataAccess
{
    public class NoRESTContext : DbContext
    {
        private static bool DebugHackApplied = false;

        public NoRESTContext(DbContextOptions<NoRESTContext> options)
            : base(options) 
        {
            if (!DebugHackApplied)
            {
                Users.AddRange(new User[]
{
                new User { Id = 1, IdentityProviderId = "5qg4one8ve2o7024st8dks8ejq", UserName = "NoRESTUI", IsAdmin = true, IsSystemUser = true },
                new User { Id = 2, IdentityProviderId = "b9c45965-95d5-4d45-a580-bafeba254ad0", UserName = "chris.rune", IsAdmin = true, IsSystemUser = false}
});
                SaveChanges();
            }

            DebugHackApplied = true;
        }

        internal DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}