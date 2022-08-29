using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels;

namespace NoREST.DataAccess.Repositories
{
    public interface IReviewRepository
    {
        Task<int?> Create(Review review);
        Task<bool> DeleteReview(int reviewId);
        Task<ReviewProfile> GetReview(int reviewId);
        Task<ICollection<ReviewProfile>> SearchReviews(ReviewSearchFilter searchFilter);
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly IDbContextFactory<NoRESTContext> _dbFactory;
        private readonly IMapper _mapper;

        public ReviewRepository(IDbContextFactory<NoRESTContext> dbFactory, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<int?> Create(Review review)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    context.Reviews.Add(review);
                    await context.SaveChangesAsync();
                    return review.Id;
                }
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<bool> DeleteReview(int reviewId)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    var review = context.Reviews.First(r => r.Id == reviewId);
                    review.IsActive = false;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }            
        }

        public async Task<ReviewProfile> GetReview(int reviewId)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    var review = await context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
                    return _mapper.Map<ReviewProfile>(review);
                }
            }
            catch
            {
                return null;
            }            
        }

        public async Task<ICollection<ReviewProfile>> SearchReviews(int userId)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var reviews = await context.Reviews.Where(r => r.CreatedById == userId).ToListAsync();
                return reviews.Select(_mapper.Map<ReviewProfile>).ToList();
            }         
        }

        public async Task<ICollection<ReviewProfile>> SearchReviews(ReviewSearchFilter searchFilter)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var query = context.Reviews.AsNoTracking();

                if (searchFilter.UserId.HasValue)
                    query = query.Where(r => r.CreatedById == searchFilter.UserId.Value);

                if (searchFilter.RestaurantId.HasValue)
                    query = query.Where(r => r.RestaurantId == searchFilter.RestaurantId.Value);

                if (searchFilter.IsActive.HasValue)
                    query = query.Where(r => r.IsActive == searchFilter.IsActive.Value);

                if (!string.IsNullOrWhiteSpace(searchFilter.RestaurantName))
                    query = query.Where(r => r.Restaurant.Name == searchFilter.RestaurantName);

                var reviews = await query.ToListAsync();
                return reviews.Select(_mapper.Map<ReviewProfile>).ToList();
            }         
        }

        
    }
}
