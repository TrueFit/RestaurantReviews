using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RstrntAPI.Repository;
using RstrntAPI.Repository.Repositories;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public class UserService : IUserService
    {
        public UserDTO Get(int userId)
        {
            return RepoRegistry.Get<IUserRepository>().Get(userId);
        }

        public UserDTO Get(string acctName)
        {
            return RepoRegistry.Get<IUserRepository>().Get(acctName);
        }

        public UserDTO Create(UserDTO user)
        {
            return RepoRegistry.Get<IUserRepository>().Create(user);
        }

        public UserDTO Update(UserDTO user)
        {
            return RepoRegistry.Get<IUserRepository>().Update(user);
        }

        public bool Delete(UserDTO user)
        {
            return RepoRegistry.Get<IUserRepository>().Delete(user);
        }

        public bool Delete(int userId)
        {
            return RepoRegistry.Get<IUserRepository>().Delete(userId);
        }

        public List<UserDTO> GetAll()
        {
            return RepoRegistry.Get<IUserRepository>().GetAll();
        }

        public List<UserDTO> Find(string term)
        {
            return RepoRegistry.Get<IUserRepository>().Find(term);
        }

        public List<UserDTO> ListByHometown(int cityId)
        {
            return RepoRegistry.Get<IUserRepository>().ListByHometown(cityId);
        }

        public List<UserDTO> ListByHometown(CityDTO city)
        {
            return RepoRegistry.Get<IUserRepository>().ListByHometown(city);
        }
    }
}
