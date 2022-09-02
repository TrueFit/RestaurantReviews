using MediatR;
using ErrorOr;
using TrueFoodReviews.Application.Authentication.Common;

namespace TrueFoodReviews.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;