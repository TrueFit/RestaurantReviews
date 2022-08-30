using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoREST.DataAccess.Entities;
using NoREST.Models.DomainModels;
using NoREST.Models.ViewModels.Profile;

namespace NoREST.DataAccess.Repositories
{
    public interface IRestaurantRepository 
    {
        Task<int?> Create(Restaurant restaurant);
        Task<RestaurantProfile> GetRestaurant(int restaurantId);
        Task<ICollection<RestaurantProfile>> SearchRestaurants(RestaurantSearchFilter searchFilter);
    }

    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IDbContextFactory<NoRESTContext> _dbFactory;
        private readonly IMapper _mapper;

        public RestaurantRepository(IDbContextFactory<NoRESTContext> dbFactory, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<int?> Create(Restaurant restaurant)
        {
            try
            {
                using (var context = _dbFactory.CreateDbContext())
                {
                    context.Restaurants.Add(restaurant);
                    await context.SaveChangesAsync();
                    return restaurant.Id;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RestaurantProfile> GetRestaurant(int restaurantId)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var restaurant = await context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
                return _mapper.Map<RestaurantProfile>(restaurant);
            }
        }

        public async Task<ICollection<RestaurantProfile>> SearchRestaurants(RestaurantSearchFilter searchFilter)
        {
            using (var context = _dbFactory.CreateDbContext())
            {
                var query = context.Restaurants.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(searchFilter.City))
                    query = query.Where(x => x.City == searchFilter.City);

                var restaurants = await query.ToListAsync();
                return restaurants.Select(_mapper.Map<RestaurantProfile>).ToList();
            }
        }
    }
}
