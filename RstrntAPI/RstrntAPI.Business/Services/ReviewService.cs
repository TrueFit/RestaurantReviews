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
    public class ReviewService : IReviewService
    {
        public ReviewDTO Get(int reviewId)
        {
            return RepoRegistry.Get<IReviewRepository>().Get(reviewId);
        }

        public ReviewDTO Create(ReviewDTO review)
        {
            return RepoRegistry.Get<IReviewRepository>().Create(review);
        }

        public ReviewDTO Update(ReviewDTO review)
        {
            return RepoRegistry.Get<IReviewRepository>().Update(review);
        }

        public bool Delete(ReviewDTO review)
        {
            return RepoRegistry.Get<IReviewRepository>().Delete(review);
        }

        public bool Delete(int reviewId)
        {
            return RepoRegistry.Get<IReviewRepository>().Delete(reviewId);
        }

        public List<ReviewDTO> GetAll()
        {
            return RepoRegistry.Get<IReviewRepository>().GetAll();
        }

        public List<ReviewDTO> Find(string term)
        {
            return RepoRegistry.Get<IReviewRepository>().Find(term);
        }

        public List<ReviewDTO> ListByLocation(int location_id)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByLocation(location_id);
        }

        public List<ReviewDTO> ListByLocation(LocationDTO location)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByLocation(location);
        }

        public List<ReviewDTO> ListByCity(int cityId)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByCity(cityId);
        }

        public List<ReviewDTO> ListByCity(CityDTO city)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByCity(city);
        }

        public List<ReviewDTO> ListByRestaurant(int restaurantId)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByRestaurant(restaurantId);
        }

        public List<ReviewDTO> ListByRestaurant(CityDTO city)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByRestaurant(city);
        }

        public List<ReviewDTO> ListByUser(int userId)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByUser(userId);
        }

        public List<ReviewDTO> ListByUser(UserDTO user)
        {
            return RepoRegistry.Get<IReviewRepository>().ListByUser(user);
        }
    }
}
