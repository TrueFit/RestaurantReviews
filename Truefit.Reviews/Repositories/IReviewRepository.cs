using System;
using System.Collections.Generic;
using Truefit.Reviews.Models;

namespace Truefit.Reviews.Repositories
{
    public interface IReviewRepository
    {
        ReviewModel GetByGuid(Guid guid);
        IEnumerable<ReviewModel> GetByEntity(Guid guid);
    }
}
