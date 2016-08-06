using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;
using RstrntAPI.DataAccess.Massive;
using System.Collections.Generic;
using System.Linq;
using RstrntAPI.Repository.Transforms;
using System.Dynamic;

namespace RstrntAPI.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region CRUD

        public UserDTO Get(int userId)
        {
            var table = new DataAccess.Models.Users();
            return table.All(where: "id=@0", args: userId).Select(x => ((ExpandoObject)x).ToEntity<UsersEntity>().ToDTO()).FirstOrDefault();
        }

        public UserDTO Get(string acctName)
        {
            var table = new DataAccess.Models.Users();
            return table.All(where: "acct_name=@0", args: acctName).Select(x => ((ExpandoObject)x).ToEntity<UsersEntity>().ToDTO()).FirstOrDefault();
        }

        public UserDTO Create(UserDTO user)
        {
            var table = new DataAccess.Models.Users();
            var returnValue = table.Insert(user.ToEntity());

            return ((ExpandoObject)returnValue).ToEntity<UsersEntity>().ToDTO();
        }

        public UserDTO Update(UserDTO user)
        {
            var table = new DataAccess.Models.Users();
            var returnValue = table.Update(user.ToEntity(), user.Id);
            return Get(user.Id.Value);
        }

        public bool Delete(UserDTO user)
        {
            var table = new DataAccess.Models.Users();
            return Delete(user.Id.Value);
        }

        public bool Delete(int userId)
        {
            var table = new DataAccess.Models.Users();
            var value = Get(userId);
            if (value != null)
                return table.Delete(userId) != 0 ? true : false;
            return false;
        }

        #endregion

        public List<UserDTO> GetAll()
        {
            var table = new DataAccess.Models.Users();
            return table.All().Select(x => ((ExpandoObject)x).ToEntity<UsersEntity>().ToDTO()).ToList();
        }

        public List<UserDTO> Find(string term)
        {
            var users = new DataAccess.Models.Users();
            return users.All().Where(x => x.acct_name.ToLower().Contains(term.ToLower()) || x.full_name.ToLower().Contains(term.ToLower())).Select(x => ((ExpandoObject)x).ToEntity<UsersEntity>().ToDTO()).ToList();
        }

        public List<UserDTO> ListByHometown(int cityId)
        {
            var rTable = new DataAccess.Models.Users();
            return rTable.All(where:"hometown = @0", args:cityId).Select(x => ((ExpandoObject)x).ToEntity<UsersEntity>().ToDTO()).ToList();
        }

        public List<UserDTO> ListByHometown(CityDTO city)
        {
            return ListByHometown(city.Id.Value);
        }
    }
}
