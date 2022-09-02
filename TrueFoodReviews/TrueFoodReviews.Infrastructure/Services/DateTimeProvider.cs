using TrueFoodReviews.Application.Common.Interfaces.Services;

namespace TrueFoodReviews.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}