using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Authentication.Common;

namespace TrueFoodReviews.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Username,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;