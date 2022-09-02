using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Users.Common;

namespace TrueFoodReviews.Application.Users.Commands.ToggleMute;

public record ToggleMuteCommand(Guid UserId) : IRequest<ErrorOr<UpdateUserResult>>;