using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Restaurants.Common;

namespace TrueFoodReviews.Application.Restaurants.Queries.List;

public class ListAllRestaurantsQueryHandler : IRequestHandler<ListAllRestaurantsQuery, ErrorOr<ListRestaurantsResult>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public ListAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    
    public async Task<ErrorOr<ListRestaurantsResult>> Handle(ListAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = _restaurantRepository.GetAll();

        return new ListRestaurantsResult(restaurants);
    }
}