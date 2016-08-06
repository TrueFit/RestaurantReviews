using RstrntAPI.DTO;
using RstrntAPI.DataAccess.Models;
using RstrntAPI.DataAccess.Massive;
using System.Collections.Generic;
using System.Linq;
using RstrntAPI.Repository.Transforms;
using System.Dynamic;

namespace RstrntAPI.Repository.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        #region CRUD

        public ReviewDTO Get(int reviewId)
        {
            var table = new DataAccess.Models.Reviews();
            return table.All(where: "id=@0", args: reviewId).Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).FirstOrDefault();
        }

        public ReviewDTO Create(ReviewDTO review)
        {
            var table = new DataAccess.Models.Reviews();
            var returnValue = table.Insert(review.ToEntity());

            return ((ExpandoObject)returnValue).ToEntity<ReviewsEntity>().ToDTO();
        }

        public ReviewDTO Update(ReviewDTO review)
        {
            var table = new DataAccess.Models.Reviews();
            var returnValue = table.Update(review.ToEntity(), review.Id);
            return Get(review.Id.Value);
        }

        public bool Delete(ReviewDTO review)
        {
            var table = new DataAccess.Models.Reviews();
            return Delete(review.Id.Value);
        }

        public bool Delete(int reviewId)
        {
            var table = new DataAccess.Models.Reviews();
            var review = Get(reviewId);
            if(review != null)
                return table.Delete(review.Id) != 0 ? true : false;
            return false;
        }

        #endregion

        public List<ReviewDTO> GetAll()
        {
            var table = new DataAccess.Models.Reviews();
            return table.All().Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> Find(string term)
        {
            var reviews = new DataAccess.Models.Reviews();
            return reviews.All().Where(x => x.subject.ToLower().Contains(term.ToLower()) || x.body.ToLower().Contains(term.ToLower())).Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> ListByLocation(int location_id)
        {
            var rTable = new DataAccess.Models.Reviews();
            return rTable.All(where: "location_id = @0", args: location_id).Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> ListByLocation(LocationDTO location)
        {
            return ListByLocation(location.Id.Value);
        }

        public List<ReviewDTO> ListByCity(int cityId)
        {
            var rTable = new DataAccess.Models.Reviews();
            var reviews = rTable.Query(
                "SELECT Reviews.* FROM Reviews INNER JOIN Locations ON Locations.id = Reviews.location_id WHERE Location.city_id = @0",
                cityId);
            return reviews.Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> ListByCity(CityDTO city)
        {
            return ListByCity(city.Id.Value);
        }

        public List<ReviewDTO> ListByRestaurant(int restaurantId)
        {
            var rTable = new DataAccess.Models.Reviews();
            var restaurants = rTable.Query(
                "SELECT Reviews.* FROM Reviews INNER JOIN Locations ON Locations.id = Reviews.location_id WHERE Location.restaurant_id = @0",
                restaurantId);
            return restaurants.Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> ListByRestaurant(CityDTO city)
        {
            return ListByCity(city.Id.Value);
        }

        public List<ReviewDTO> ListByUser(int userId)
        {
            var rTable = new DataAccess.Models.Reviews();
            return rTable.All(where: "user_id = @0", args: userId).Select(x => ((ExpandoObject)x).ToEntity<ReviewsEntity>().ToDTO()).ToList();
        }

        public List<ReviewDTO> ListByUser(UserDTO user)
        {
            return ListByUser(user.Id.Value);
        }
    }
}
