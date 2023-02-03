using System;
namespace RestaurantReviews.Domain.Seedwork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
