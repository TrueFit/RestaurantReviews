using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Authentication.Common;
using TrueFoodReviews.Application.Common.Interfaces.Authentication;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Domain.Common.Errors;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : 
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (_userRepository.GetUserByUsername(command.Username) is not null)
        {
            return Errors.User.DuplicateUsername;
        }
        
        // Create user (generate unique ID)
        var user = new User
        {
            FirstName = command.FirstName, 
            LastName = command.LastName, 
            Email = command.Email, 
            Username = command.Username,
            Password = command.Password
        };
        
        _userRepository.Add(user);
        
        // Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(
            user,
            token);
    }
}