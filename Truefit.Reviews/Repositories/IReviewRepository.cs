using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Reviews.Models;

namespace Truefit.Reviews.Repositories
{
    public interface IReviewRepository
    {
        /// <summary>
        /// Gets a Review by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<ReviewModel> GetByGuid(Guid guid);

        /// <summary>
        /// Gets all Reviews for an Entity
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IEnumerable<ReviewModel>> GetByEntity(Guid entityId);

        /// <summary>
        /// Get all Reviews by User
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IEnumerable<ReviewModel>> GetByUser(Guid userId);

        /// <summary>
        /// Inserts Review into the Repository
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        Task Insert(ReviewModel review);

        /// <summary>
        /// Deletes a Review from the Repository
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task Delete(Guid guid);
    }
}
