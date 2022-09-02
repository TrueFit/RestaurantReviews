using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Users.Common;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Users.Commands.Add;

public record UpdateUserCommand(
    User User) : IRequest<ErrorOr<UpdateUserResult>>;