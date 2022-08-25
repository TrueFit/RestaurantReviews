using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserByIdentityProviderId(string identityProviderId);
        Task<User> GetUser(int userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly NoRESTContext _dbContext;

        public UserRepository(NoRESTContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            _dbContext.Add(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUser(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByIdentityProviderId(string identityProviderId)
        {
            var allUsers = await _dbContext.Users.ToListAsync();

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.IdentityProviderId == identityProviderId);
        }
    }
}
