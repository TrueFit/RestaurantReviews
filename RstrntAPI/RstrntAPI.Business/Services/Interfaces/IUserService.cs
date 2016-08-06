﻿using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public interface IUserService
    {
        UserDTO Create(UserDTO user);
        bool Delete(UserDTO user);
        bool Delete(int userId);
        List<UserDTO> Find(string term);
        UserDTO Get(string acctName);
        UserDTO Get(int userId);
        List<UserDTO> GetAll();
        List<UserDTO> ListByHometown(int cityId);
        List<UserDTO> ListByHometown(CityDTO city);
        UserDTO Update(UserDTO user);
    }
}