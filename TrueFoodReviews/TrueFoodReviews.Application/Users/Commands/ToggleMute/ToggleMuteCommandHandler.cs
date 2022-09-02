using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Users.Common;
using TrueFoodReviews.Domain.Common.Errors;

namespace TrueFoodReviews.Application.Users.Commands.ToggleMute;

public class ToggleMuteCommandHandler : IRequestHandler<ToggleMuteCommand, ErrorOr<UpdateUserResult>>
{
    private readonly IUserRepository _userRepository;

    public ToggleMuteCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UpdateUserResult>> Handle(ToggleMuteCommand command, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserById(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }
        
        _userRepository.ToggleMute(command.UserId);

        return new UpdateUserResult(user);
    }
}