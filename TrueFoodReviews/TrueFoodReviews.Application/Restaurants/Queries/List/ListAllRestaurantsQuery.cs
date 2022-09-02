using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Restaurants.Common;

namespace TrueFoodReviews.Application.Restaurants.Queries.List;

public record ListAllRestaurantsQuery() : IRequest<ErrorOr<ListRestaurantsResult>>;