using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Authentication.Common;
using TrueFoodReviews.Application.Common.Interfaces.Authentication;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Domain.Common.Errors;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        // Check if user exists
        if (_userRepository.GetUserByUsername(query.Username) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // Validate password
        if (user.Password != query.Password)
        {
            return Errors.Authentication.InvalidCredentials;
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(
            user,
            token);
    }
}