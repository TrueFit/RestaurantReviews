using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantReviews.Core;
using RestaurantReviews.Data;

namespace RestaurantReviews.Service
{
    public class UserService: BaseService
    {
        private UserRepository _userRepo;

        public UserService()
        {
            _userRepo = new UserRepository();
        }

        public ServiceCallResult CreateNewUser(User entity)
        {
            entity.Validate();

            ServiceCallResult srv = new ServiceCallResult();
            srv.ValidationErrors.AddRange(entity.ValidationErrors);

            if (entity.IsValid)
            {
                try
                {
                    //checking for duplicate records first. Email must be unique
                    User dupCheck = _userRepo.Filter(u => u.Email == entity.Email).FirstOrDefault();
                    if (!(dupCheck == null))
                    {
                        srv.ValidationErrors.Add("Email " + entity.Email + " already exists in the database");
                    }
                    else
                    {
                        int? entityID = _userRepo.AddEntity(entity);
                        if (entityID == null)
                            srv.ValidationErrors.Add("Error inserting the record into the database");
                        else
                            srv.ResultObject = entityID;
                    }
                }
                catch (Exception ex)
                {
                    srv.ValidationErrors.Add("Database error: " + ex.Message);
                }
            }
            return srv;
        }

        public ServiceCallResult GetAllUsers()
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _userRepo.GetAll();
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetUserByEmail(string email)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                IEnumerable<User> results = _userRepo.Filter(u => u.Email == email);
                if (results.Count() > 0)
                    srv.ResultObject = results.First();
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult UpdateUser(User usr)
        {
            usr.Validate();

            ServiceCallResult srv = new ServiceCallResult();
            srv.ValidationErrors.AddRange(usr.ValidationErrors);

            if (usr.IsValid)
            {
                try
                {
                    _userRepo.UpdateEntity(usr);
                }
                catch (Exception ex)
                {
                    srv.ValidationErrors.Add("Database error: " + ex.Message);
                }
            }
            return srv;
        }

        public ServiceCallResult DeleteReview(int reviewId)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                _userRepo.DeleteUserReview(reviewId);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

        public ServiceCallResult GetUserReviews(int userId)
        {
            ServiceCallResult srv = new ServiceCallResult();
            try
            {
                srv.ResultObject = _userRepo.GetUserReviews(userId);
            }
            catch (Exception ex)
            {
                srv.ValidationErrors.Add("Database error: " + ex.Message);
            }
            return srv;
        }

    }
}
