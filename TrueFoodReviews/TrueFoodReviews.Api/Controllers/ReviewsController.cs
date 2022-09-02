using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrueFoodReviews.Application.Reviews.Commands.Add;
using TrueFoodReviews.Application.Reviews.Commands.Delete;
using TrueFoodReviews.Application.Reviews.Queries.List;
using TrueFoodReviews.Contracts.Reviews;

namespace TrueFoodReviews.Api.Controllers;

[Route("reviews")]
public class ReviewsController : ApiController
{
    private readonly ISender _mediator;

    public ReviewsController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds a new review by the user to the given restaurant.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Review created successfully.</response>
    /// <response code="404">If User or Restaurant were not found.</response>
    /// <response code="409">If a duplicate review already exists.</response>
    [HttpPost("add")]
    public async Task<IActionResult> Add(AddReviewRequest request)
    {
        var command = new AddReviewCommand(
            request.RestaurantId,
            request.UserId,
            request.Title,
            request.Content,
            request.Rating);
        
        var reviewResult = await _mediator.Send(command);
        
        return reviewResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Returns a list of all reviews for the given user.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Returns the list of all reviews by user.</response>
    [HttpGet("reviewsByUser/{id}")]
    public async Task<IActionResult> ListReviewsByUser(Guid id)
    {
        var reviewsResult = await _mediator.Send(new ListReviewsByUserIdQuery(id));
        
        return reviewsResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Returns a list of all reviews for the given restaurant.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Returns a list of all reviews for the given restaurant.</response>
    [HttpGet("reviewsByRestaurant/{id}")]
    public async Task<IActionResult> ListReviewsByRestaurant(int id)
    {
        var reviewsResult = await _mediator.Send(new ListReviewsByRestaurantIdQuery(id));
        
        return reviewsResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Deletes a given review.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Review deleted successfully.</response>
    /// <response code="404">Review not found.</response>
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(DeleteReviewRequest request)
    {
        var reviewResult = await _mediator.Send(new DeleteReviewCommand(request.ReviewId));
        
        return reviewResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
}