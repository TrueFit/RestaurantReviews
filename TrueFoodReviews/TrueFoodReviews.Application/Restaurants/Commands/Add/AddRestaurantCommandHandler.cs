using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Restaurants.Common;
using TrueFoodReviews.Domain.Common.Errors;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Restaurants.Commands.Add;

public class AddRestaurantCommandHandler : 
    IRequestHandler<AddRestaurantCommand, ErrorOr<AddRestaurantResult>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public AddRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<ErrorOr<AddRestaurantResult>> Handle(AddRestaurantCommand request, CancellationToken cancellationToken)
    {
        // check if restaurant with same name already exists
        var existingRestaurants = _restaurantRepository.GetRestaurantByName(request.Name);
        if (existingRestaurants.Count > 0 && existingRestaurants.Exists(r => r.Address == request.Address))
        {
            return Errors.Restaurants.DuplicateRestaurant;
        }

        var restaurant = new Restaurant
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Description = request.Description,
        };
        
        _restaurantRepository.Add(restaurant);
        
        return new AddRestaurantResult(restaurant);
    }
}