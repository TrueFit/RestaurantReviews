using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrueFoodReviews.Application.Authentication.Commands.Register;
using TrueFoodReviews.Application.Authentication.Common;
using TrueFoodReviews.Application.Authentication.Queries.Login;
using TrueFoodReviews.Contracts.Authentication;
using TrueFoodReviews.Domain.Common.Errors;

namespace TrueFoodReviews.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Registers a new User. Provides a JWT token.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns the User object and a JWT token.</response>
    /// <response code="409">If the username is already taken.</response>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, 
            request.LastName, 
            request.Email, 
            request.Username,
            request.Password);

        var registerResult = await _mediator.Send(command);

        return registerResult.Match(
            authResult => Ok(MapToAuthenticationResult(authResult)),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Logs in a User. Provides a JWT token.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns the User object and a JWT token.</response>
    /// <response code="401">If the provided credentials were invalid.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Username, request.Password);
        var loginResult = await _mediator.Send(query);
            
        if (loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: loginResult.FirstError.Description);
        }
            
        return loginResult.Match(
            authResult => Ok(MapToAuthenticationResult(authResult)),
            errors => Problem(errors));
    }
    
    private static AuthenticationResponse MapToAuthenticationResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.User.Username,
            authResult.Token);
    }
}