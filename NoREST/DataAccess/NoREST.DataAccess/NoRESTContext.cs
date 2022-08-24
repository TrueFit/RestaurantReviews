using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;

namespace NoREST.DataAccess
{
    public class NoRESTContext : DbContext
    {
        public NoRESTContext(DbContextOptions<NoRESTContext> options)
            : base(options) { }

        internal DbSet<User> Users { get; set; }
    }
}