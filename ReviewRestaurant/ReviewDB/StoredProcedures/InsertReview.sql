CREATE PROCEDURE [dbo].[InsertReview]
@UserID INT,
@RestaurantID INT,
@RatingID TINYINT,
@ReviewText NVARCHAR(1000),
@ReviewID INT OUTPUT

AS
SET NOCOUNT ON

INSERT INTO dbo.Reviews(UserID, RestaurantID, RatingID, ReviewText) 
VALUES (@UserID, @RestaurantID, @RatingID, @ReviewText)

SET @ReviewID = SCOPE_IDENTITY()