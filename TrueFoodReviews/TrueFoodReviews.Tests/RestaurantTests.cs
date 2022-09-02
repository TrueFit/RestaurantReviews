using ErrorOr;
using FluentAssertions;
using Moq;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Restaurants.Commands.Add;
using TrueFoodReviews.Application.Restaurants.Common;
using TrueFoodReviews.Application.Restaurants.Queries.List;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Tests;

public class RestaurantTests
{
    public RestaurantTests()
    {
        var mockRepo = new Mock<IRestaurantRepository>();

        var allData = MockData.Restaurants.GetRestaurants();
        
        mockRepo.Setup(mr => mr.GetAll()).Returns(allData);
        
        mockRepo.Setup(mr => mr.GetRestaurantByCity(
                It.IsAny<string>())).Returns((string s) => allData.Where(r => r.City == s).ToList());
        
        mockRepo.Setup(mr => mr.GetRestaurantByName(
            It.IsAny<string>())).Returns((string s) => allData.Where(r => r.Name == s).ToList());
        
        mockRepo.Setup(mr => mr.Add(It.IsAny<Restaurant>())).Callback((Restaurant r) => allData.Add(r));
        
        _restaurantRepository = mockRepo.Object;
    }
    
    private readonly IRestaurantRepository _restaurantRepository;
    
    [Fact]
    public void GetAll_ShouldReturn2()
    {
        var handler = new ListAllRestaurantsQueryHandler(_restaurantRepository);
        var result = handler.Handle(new ListAllRestaurantsQuery(), default).Result;
        
        result.Should().BeOfType<ErrorOr<ListRestaurantsResult>>();
        result.Value.Should().BeOfType<ListRestaurantsResult>();
        result.Value.Restaurants.Count.Should().Be(2);
    }

    [Fact]
    public void GetByCity_ShouldReturn1()
    {
        var handler = new ListRestaurantsByCityQueryHandler(_restaurantRepository);
        var result = handler.Handle(new ListRestaurantsByCityQuery("City 2"), default).Result;
        
        result.Should().BeOfType<ErrorOr<ListRestaurantsResult>>();
        result.Value.Should().BeOfType<ListRestaurantsResult>();
        result.Value.Restaurants.Count.Should().Be(1);
    }

    [Fact]
    public void Add_ShouldReturn3()
    {
        var command = new AddRestaurantCommand("New Restaurant", "Address 1", "City 1", "State 1", "A new restaurant!");
        var handler = new AddRestaurantCommandHandler(_restaurantRepository);
        var result = handler.Handle(command, default).Result;
        
        result.Should().BeOfType<ErrorOr<AddRestaurantResult>>();
        result.Value.Should().BeOfType<AddRestaurantResult>();
        _restaurantRepository.GetAll().Count.Should().Be(3);
    }
}