using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Domain.AggregatesModel.RestaurantAggregate;
using RestaurantReviews.Domain.AggregatesModel.ReviewAggregate;
using RestaurantReviews.Domain.AggregatesModel.UserAggregate;
using RestaurantReviews.Infrastructure;
using RestaurantReviews.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantReviewRepository, RestaurantReviewRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// TODO: remove unneeded inmemory package when moving to actual connection
builder.Services.AddDbContext<ReviewsContext>(options => { options.UseInMemoryDatabase(databaseName: "Reviews"); });
//builder.Services.AddDbContext<ReviewsContext>(options =>
//{
//    // get connection string and connect
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
