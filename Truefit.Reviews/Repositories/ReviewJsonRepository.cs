using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biggy.Core;
using Biggy.Data.Json;
using Truefit.Reviews.Models;

namespace Truefit.Reviews.Repositories
{
    public class ReviewJsonRepository : IReviewRepository
    {
        private readonly BiggyList<ReviewModel> _reviews;
        public ReviewJsonRepository(JsonDbCore dbCore)
        {
            var store = new JsonStore<ReviewModel>(dbCore);
            this._reviews = new BiggyList<ReviewModel>(store);
        }

        public async Task<ReviewModel> GetByGuid(Guid guid)
        {
            return await Task.FromResult(this._reviews.FirstOrDefault(x => x.Guid == guid));
        }

        public async Task<IEnumerable<ReviewModel>> GetByEntity(Guid entityId)
        {
            return await Task.FromResult(this._reviews.Where(x => x.EntityGuid == entityId));
        }

        public async Task<IEnumerable<ReviewModel>> GetByUser(Guid userId)
        {
            return await Task.FromResult(this._reviews.Where(x => x.UserGuid == userId));
        }

        public async Task Insert(ReviewModel review)
        {
            await Task.Run(() => this._reviews.Add(review));
        }

        public async Task Delete(Guid guid)
        {
            var review = this._reviews.FirstOrDefault(x => x.Guid == guid);
            await Task.Run(() => this._reviews.Remove(review));
        }
    }
}
