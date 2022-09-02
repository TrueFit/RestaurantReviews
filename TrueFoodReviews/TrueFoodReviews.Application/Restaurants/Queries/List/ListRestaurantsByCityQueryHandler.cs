using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Restaurants.Common;

namespace TrueFoodReviews.Application.Restaurants.Queries.List;

public class ListRestaurantsByCityQueryHandler : IRequestHandler<ListRestaurantsByCityQuery, ErrorOr<ListRestaurantsResult>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public ListRestaurantsByCityQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<ErrorOr<ListRestaurantsResult>> Handle(ListRestaurantsByCityQuery query, CancellationToken cancellationToken)
    {
        var restaurants = _restaurantRepository.GetRestaurantByCity(query.City);

        return new ListRestaurantsResult(restaurants);
    }
}