using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Restaurants.Common;

namespace TrueFoodReviews.Application.Restaurants.Commands.Add;

public record AddRestaurantCommand(
    string Name,
    string Address,
    string City,
    string State,
    string Description) : IRequest<ErrorOr<AddRestaurantResult>>;