CREATE PROCEDURE [dbo].[InsertRestaurantReview]
	@UserName NVARCHAR(50),
	@RestaurantName NVARCHAR(50),
	@RatingDescription NVARCHAR(50),
	@ReviewText NVARCHAR(1000),
	@ReviewID INT OUTPUT
AS
SET NOCOUNT ON

DECLARE @UserID INT
DECLARE @RestaurantID INT
DECLARE @RatingID TINYINT

EXEC	[dbo].[GetUserIDByName]
		@UserName = @UserName,
		@UserID = @UserID OUTPUT

EXEC	[dbo].[GetRestaurantIDByRestaurantName]
		@RestaurantName = @RestaurantName,
		@RestaurantID = @RestaurantID OUTPUT

EXEC	[dbo].[GetRatingIDByRating]
		@RatingDescription = @RatingDescription,
		@RatingID = @RatingID OUTPUT

INSERT INTO dbo.Reviews(UserID, RestaurantID, RatingID, ReviewText, ReviewDate) 
VALUES (@UserID, @RestaurantID, @RatingID, @ReviewText, GETUTCDATE())

SET @ReviewID = SCOPE_IDENTITY()