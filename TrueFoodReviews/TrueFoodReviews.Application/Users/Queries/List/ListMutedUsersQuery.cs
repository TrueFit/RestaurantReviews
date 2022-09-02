using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Users.Common;

namespace TrueFoodReviews.Application.Users.Queries.List;

public record ListMutedUsersQuery() : IRequest<ErrorOr<ListUsersResult>>;