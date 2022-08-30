using AutoMapper;
using Microsoft.Extensions.Logging;
using NoREST.DataAccess.Entities;
using NoREST.DataAccess.Repositories;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Creation;
using NoREST.Models.ViewModels.Outgoing;
using NoREST.Models.ViewModels.Profile;

namespace NoREST.Domain
{
    public interface IRestaurantLogic
    {
        Task<RestaurantProfile> GetRestaurant(int restaurantId);
        Task<RestaurantProfile> Create(RestaurantCreation restaurantCreation, UserProfile userProfile);
        Task<ApiResult<ReviewProfile>> CreateReview(ReviewCreation reviewCreation, UserProfile userProfile);
        Task<ApiResult<bool>> DeleteReview(int reviewId, UserProfile userProfile);
        Task<ICollection<ReviewProfile>> SearchReviews(ReviewSearchFilter searchFilter);
        Task<ICollection<RestaurantProfile>> SearchRestaurants(RestaurantSearchFilter searchFilter);
    }

    public class RestaurantLogic : IRestaurantLogic
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRestaurantLogic> _logger;
        private readonly IPermissionLogic _permissionLogic;
        private readonly IAuditLogic _auditLogic;

        public RestaurantLogic(
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            ILogger<IRestaurantLogic> logger,
            IReviewRepository reviewRepository,
            IPermissionLogic permissionLogic,
            IAuditLogic auditLogic)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _logger = logger;
            _reviewRepository = reviewRepository;
            _permissionLogic = permissionLogic;
            _auditLogic = auditLogic;
        }

        public async Task<RestaurantProfile> Create(RestaurantCreation restaurantCreation, UserProfile createdByUser)
        {
            try
            {
                var profile = _mapper.Map<RestaurantProfile>(restaurantCreation);
                _auditLogic.SetAuditAndOwnershipValues<RestaurantProfile, UserProfile, int>(profile, createdByUser, true);
                var createdId = await _restaurantRepository.Create(_mapper.Map<Restaurant>(profile));
                if (createdId == null) return null;
                profile.Id = createdId.Value;
                // We don't populate the CreatedBy user on return (just the Id)... maybe UI would want this?
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not create restaurant with name {restaurantCreation.Name}");
                return null;
            }
            
        }

        public async Task<ApiResult<ReviewProfile>> CreateReview(ReviewCreation reviewCreation, UserProfile userProfile)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetRestaurant(reviewCreation.RestaurantId);

                if (restaurant == null)
                {
                    return new ApiResult<ReviewProfile>(null, false, $"Restaurant with Id {reviewCreation.RestaurantId} does not exist.", System.Net.HttpStatusCode.NotFound);
                }

                if (restaurant.BannedUsers.Any(bu => bu.UserId == userProfile.Id))
                {
                    return new ApiResult<ReviewProfile>(null, false, "User has been blocked from posting reviews for this restaurant", System.Net.HttpStatusCode.Unauthorized);
                }

                var review = _mapper.Map<ReviewProfile>(reviewCreation);
                _auditLogic.SetAuditAndOwnershipValues<ReviewProfile, UserProfile, int>(review, userProfile, true);
                var createdId = await _reviewRepository.Create(_mapper.Map<Review>(review));
                if (createdId == null)
                {
                    return new ApiResult<ReviewProfile>(null, false, "Failed to save review", System.Net.HttpStatusCode.Conflict);
                }

                review.Id = createdId.Value;
                
                return new ApiResult<ReviewProfile>(review);                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not create review");
                return new ApiResult<ReviewProfile>(null, false, "Unexpected exception", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult<bool>> DeleteReview(int reviewId, UserProfile userProfile)
        {
            try
            {
                var review = await _reviewRepository.GetReview(reviewId);
                if (review == null)
                {
                    return new ApiResult<bool>(false, false, $"Review with id {reviewId} does not exist", System.Net.HttpStatusCode.NotFound);
                }

                if (!_permissionLogic.CanModify(review, userProfile))
                {
                    return new ApiResult<bool>(false, false, $"User is not authorized to delete review with Id {reviewId}", System.Net.HttpStatusCode.Unauthorized);
                }
                _auditLogic.SetAuditValues(review);
                var success = await _reviewRepository.DeleteReview(reviewId);

                if (!success)
                {
                    return new ApiResult<bool>(false, false, "Failed to delete review", System.Net.HttpStatusCode.InternalServerError);
                }

                return new ApiResult<bool>(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error attempting to delete review with Id {reviewId}");
                return new ApiResult<bool>(false, false, "Unexpected error", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<RestaurantProfile> GetRestaurant(int restaurantId)
        {
            try
            {
                return await _restaurantRepository.GetRestaurant(restaurantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected exception while fetching restaurant with Id {restaurantId}.");
                return null;
            }
        }

        public async Task<ICollection<RestaurantProfile>> SearchRestaurants(RestaurantSearchFilter searchFilter)
        {
            return await _restaurantRepository.SearchRestaurants(searchFilter);
        }

        public async Task<ICollection<ReviewProfile>> SearchReviews(ReviewSearchFilter searchFilter)
        {
            return await _reviewRepository.SearchReviews(searchFilter);
        }
    }
}