using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Reviews.Models;
using Truefit.Reviews.Repositories;

namespace Truefit.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            this._reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewModel>> GetByEntity(Guid entityId)
        {
            return await this._reviewRepository.GetByEntity(entityId);
        }

        public async Task<IEnumerable<ReviewModel>> GetByUser(Guid userId)
        {
            return await this._reviewRepository.GetByUser(userId);
        }

        public async Task AddUserReview(ReviewModel review)
        {
            review.Guid = Guid.NewGuid();
            review.DateCreated = DateTime.Now;
            await this._reviewRepository.Insert(review);
        }

        public async Task<bool> RemoveUserReview(Guid reviewId, Guid userId)
        {
            var review = await this._reviewRepository.GetByGuid(reviewId);
            if (review == null || review.UserGuid != userId)
                return false;

            await this._reviewRepository.Delete(reviewId);
            return true;
        }
    }
}
