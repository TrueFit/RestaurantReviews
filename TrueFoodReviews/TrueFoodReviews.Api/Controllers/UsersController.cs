using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrueFoodReviews.Application.Users.Commands.ToggleMute;

namespace TrueFoodReviews.Api.Controllers;

[Route("users")]
public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Toggles the mute status of the user. A value of <c>true</c> will prevent the user from posting a new review.
    /// </summary>
    /// <param name="id">The Id of the User to Toggle Mute status.</param>
    /// <returns></returns>
    [HttpPost("toggleMute")]
    public async Task<IActionResult> ToggleMute(Guid id)
    {
        var reviewsResult = await _mediator.Send(new ToggleMuteCommand(id));
        
        return reviewsResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
}