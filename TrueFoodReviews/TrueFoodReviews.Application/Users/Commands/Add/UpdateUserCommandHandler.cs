using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Users.Common;
using TrueFoodReviews.Domain.Common.Errors;

namespace TrueFoodReviews.Application.Users.Commands.Add;

public class UpdateUserCommandHandler : 
    IRequestHandler<UpdateUserCommand, ErrorOr<UpdateUserResult>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UpdateUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // find existing user
        var user = _userRepository.GetUserByUsername(request.User.Username);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        user.FirstName = request.User.FirstName;
        user.LastName = request.User.LastName;
        user.Email = request.User.Email;
        user.Username = request.User.Username;
        user.Password = request.User.Password;
        
        // TODO: Determine if we need to do more than just utilizing reference types
            
        return new UpdateUserResult(user);
    }
}