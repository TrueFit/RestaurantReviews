using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Domain.Entities;

namespace TrueFoodReviews.Infrastructure.Persistence;

public class ReviewRepository : IReviewRepository
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static List<Review> _reviews = new();
    
    public void Add(Review review)
    {
        // TODO: Replace with actual db implementation
        review.Id = Guid.NewGuid();
        review.Timestamp = DateTime.Now;
        
        _reviews.Add(review);
    }
    
    public void Update(Review review)
    {
        var existingReview = _reviews.FirstOrDefault(r => r.Id == review.Id);
        
        if (existingReview == null) return;
        
        existingReview.Title = review.Title;
        existingReview.Content = review.Content;
        existingReview.Rating = review.Rating;
        existingReview.RestaurantId = review.RestaurantId;
        existingReview.UserId = review.UserId;
    }

    public Review? GetReviewById(Guid reviewId)
    {
        return _reviews.SingleOrDefault(r => r.Id == reviewId);
    }
    
    // public void Delete(Guid id)
    // {
    //     var existingReview = _reviews.FirstOrDefault(r => r.Id == id);
    //     
    //     if (existingReview == null) return;
    //     
    //     _reviews.Remove(existingReview);
    // }
    
    public List<Review> GetReviewsByRestaurantId(int restaurantId)
    {
        return _reviews.Where(r => r.RestaurantId == restaurantId).ToList();
    }

    public List<Review> GetReviewsByUserId(Guid userId)
    {
        return _reviews.Where(r => r.UserId == userId).ToList();
    }

    public Review? GetReviewByRestaurantIdAndUserId(int restaurantId, Guid userId)
    {
        return _reviews.SingleOrDefault(r => r.RestaurantId == restaurantId && r.UserId == userId);
    }
    
    public void Delete(Guid reviewId)
    {
        _reviews.RemoveAll(r => r.Id == reviewId);
    }
}