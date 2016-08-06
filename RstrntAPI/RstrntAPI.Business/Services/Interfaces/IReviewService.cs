using System.Collections.Generic;
using RstrntAPI.DTO;

namespace RstrntAPI.Business.Services
{
    public interface IReviewService
    {
        ReviewDTO Create(ReviewDTO review);
        bool Delete(ReviewDTO review);
        bool Delete(int reviewId);
        List<ReviewDTO> Find(string term);
        ReviewDTO Get(int reviewId);
        List<ReviewDTO> GetAll();
        List<ReviewDTO> ListByCity(int cityId);
        List<ReviewDTO> ListByCity(CityDTO city);
        List<ReviewDTO> ListByLocation(LocationDTO location);
        List<ReviewDTO> ListByLocation(int location_id);
        List<ReviewDTO> ListByRestaurant(int restaurantId);
        List<ReviewDTO> ListByRestaurant(CityDTO city);
        List<ReviewDTO> ListByUser(UserDTO user);
        List<ReviewDTO> ListByUser(int userId);
        ReviewDTO Update(ReviewDTO review);
    }
}