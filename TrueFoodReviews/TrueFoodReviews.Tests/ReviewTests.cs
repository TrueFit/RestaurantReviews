using ErrorOr;
using FluentAssertions;
using Moq;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Restaurants.Common;
using TrueFoodReviews.Application.Reviews.Commands.Add;
using TrueFoodReviews.Application.Reviews.Common;
using TrueFoodReviews.Application.Reviews.Queries.List;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Tests;

public class ReviewTests
{
    public ReviewTests()
    {
        var mockReviewRepo = new Mock<IReviewRepository>();
        var mockRestaurantRepo = new Mock<IRestaurantRepository>();
        var mockUserRepo = new Mock<IUserRepository>();

        var allReviews = MockData.Reviews.GetReviews();
        var allUsers = MockData.Users.GetUsers();
        var allRestaurants = MockData.Restaurants.GetRestaurants();

        mockReviewRepo.Setup(mr => mr.GetAll()).Returns(allReviews);
        
        mockReviewRepo.Setup(mr => mr.GetReviewsByRestaurantId(
            It.IsAny<int>())).Returns((int i) => allReviews.Where(r => r.RestaurantId == i).ToList());
        
        mockReviewRepo.Setup(mr => mr.GetReviewsByUserId(
            It.IsAny<Guid>())).Returns((Guid g) => allReviews.Where(r => r.UserId == g).ToList());
        
        mockReviewRepo.Setup(mr => mr.Add(It.IsAny<Review>())).Callback((Review r) => allReviews.Add(r));
        
        mockReviewRepo.Setup(mr => mr.Delete(It.IsAny<Guid>())).Callback((Guid g) => allReviews.RemoveAll(r => r.Id == g));
        
        mockUserRepo.Setup(mr => mr.GetUserById(It.IsAny<Guid>())).Returns((Guid g) => allUsers.FirstOrDefault(u => u.Id == g));
        
        mockRestaurantRepo.Setup(mr => mr.GetRestaurantById(It.IsAny<int>())).Returns((int i) => allRestaurants.FirstOrDefault(r => r.Id == i));
        
        _reviewRepository = mockReviewRepo.Object;
        _userRepository = mockUserRepo.Object;
        _restaurantRepository = mockRestaurantRepo.Object;
    }
    
    private readonly IReviewRepository _reviewRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUserRepository _userRepository;

    [Fact]
    public void AddReview_ShouldReturn6()
    {
        var userId = new Guid("0F8F8F8F-8F8F-8F8F-8F8F-8F8F8F8F8F8F");
        var handler = new AddReviewCommandHandler(_reviewRepository, _userRepository, _restaurantRepository);
        var command = new AddReviewCommand(3, userId, "New Review", "Content 6", 5);
        var result = handler.Handle(command, default).Result;
        
        result.Should().BeOfType<ErrorOr<AddReviewResult>>();
        result.Value.Should().BeOfType<AddReviewResult>();
        _reviewRepository.GetAll().Count.Should().Be(6);
    }
    
    [Fact]
    public void GetReviewsByUserId_ReturnsCorrectReviews()
    {
        var userId = new Guid("8F8F8F8F-8F8F-8F8F-8F8F-8F8F8F8F8F8F");
        
        var handler = new ListReviewsByUserIdQueryHandler(_reviewRepository);
        var result = handler.Handle(new ListReviewsByUserIdQuery(userId), default).Result;
        
        result.Should().BeOfType<ErrorOr<ListReviewsResult>>();
        result.Value.Should().BeOfType<ListReviewsResult>();
        result.Value.Reviews.Count.Should().Be(1);
    }
    
    [Fact]
    public void GetReviewsByRestaurantId_ReturnsCorrectReviews()
    {
        var handler = new ListReviewsByRestaurantIdQueryHandler(_reviewRepository);
        var result = handler.Handle(new ListReviewsByRestaurantIdQuery(1), default).Result;
        
        result.Should().BeOfType<ErrorOr<ListReviewsResult>>();
        result.Value.Should().BeOfType<ListReviewsResult>();
        result.Value.Reviews.Count.Should().Be(3);
    }
}