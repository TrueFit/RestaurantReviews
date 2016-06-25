using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Reviews.Models;

namespace Truefit.Reviews
{
    public interface IReviewService
    {
        /// <summary>
        /// Gets all Reviews for an Entity
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IEnumerable<ReviewModel>> GetByEntity(Guid entiyId);

        /// <summary>
        /// Gets all Reviews for a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ReviewModel>> GetByUser(Guid userId);

        /// <summary>
        /// Adds a Review from a User
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        Task AddUserReview(ReviewModel review);

        /// <summary>
        /// Removes a Review for a User
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="userId"></param>
        /// <returns>Success or Failure (true/false)</returns>
        Task<bool> RemoveUserReview(Guid reviewId, Guid userId);
    }
}
