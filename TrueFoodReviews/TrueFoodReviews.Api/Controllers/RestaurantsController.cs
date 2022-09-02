using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrueFoodReviews.Application.Restaurants.Commands.Add;
using TrueFoodReviews.Application.Restaurants.Queries.List;
using TrueFoodReviews.Contracts.Restaurants;

namespace TrueFoodReviews.Api.Controllers;

[Route("restaurants")]
public class RestaurantsController : ApiController
{
    private readonly ISender _mediator;

    public RestaurantsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Adds a new restaurant to be reviewed.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The Restaurant object.</returns>
    /// <response code="200">Returns the Restaurant object.</response>
    /// <response code="409">If a Restaurant at the same address already exists.</response>
    [HttpPost("add")]
    public async Task<IActionResult> AddRestaurant(AddRestaurantRequest request)
    {
        var command = new AddRestaurantCommand(
            request.Name,
            request.Address,
            request.City,
            request.State,
            request.Description);
        
        var restaurantResult = await _mediator.Send(command);
        
        return restaurantResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Returns a list (if any exist) of Restaurants in the provided City.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    /// <response code="200">Returns a list of Restaurants in the provided City.</response>
    [HttpGet("listByCity/{city}")]
    public async Task<IActionResult> ListRestaurantsByCity(string city)
    {
        var command = new ListRestaurantsByCityQuery(city);
        
        var restaurantResult = await _mediator.Send(command);
        
        return restaurantResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    /// <summary>
    /// Returns a list of all created Restaurants.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Returns a list of all created Restaurants.</response>
    [HttpGet("listAll")]
    public async Task<IActionResult> ListAllRestaurants()
    {
        var command = new ListAllRestaurantsQuery();
        
        var restaurantResult = await _mediator.Send(command);
        
        return restaurantResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
}