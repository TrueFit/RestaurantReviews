using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Truefit.Reviews.Models;

namespace Truefit.Reviews.Repositories
{
    public interface IReviewRepository
    {
        Task<ReviewModel> GetByGuid(Guid guid);
        Task<IEnumerable<ReviewModel>> GetByEntity(Guid guid);
        Task<IEnumerable<ReviewModel>> GetByUser(Guid guid);
        Task Insert(ReviewModel review);
        Task Delete(Guid guid);
    }
}
