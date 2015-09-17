CREATE PROCEDURE [dbo].[InsertReviewForNewRestaurant]
	@UserName NVARCHAR(50),
	@NewRestaurantName NVARCHAR(50),
	@CityName NVARCHAR(50),
	@RatingDescription NVARCHAR(50),
	@ReviewText NVARCHAR(1000),
	@ReviewID INT OUTPUT
AS
SET NOCOUNT ON

DECLARE @NewRestaurantID INT
DECLARE @UserID INT
DECLARE @RestaurantID INT
DECLARE @RatingID TINYINT

EXEC	[dbo].[AddNewRestaurant]
		@RestaurantName = @NewRestaurantName,
		@CityName = @CityName,
		@RestaurantID = @NewRestaurantID OUTPUT

EXEC	[dbo].[GetUserIDByName]
		@UserName = @UserName,
		@UserID = @UserID OUTPUT

EXEC	[dbo].[GetRatingIDByRating]
		@RatingDescription = @RatingDescription,
		@RatingID = @RatingID OUTPUT

INSERT INTO dbo.Reviews(UserID, RestaurantID, RatingID, ReviewText, ReviewDate) 
VALUES (@UserID, @NewRestaurantID, @RatingID, @ReviewText, GETUTCDATE())

SET @ReviewID = SCOPE_IDENTITY()