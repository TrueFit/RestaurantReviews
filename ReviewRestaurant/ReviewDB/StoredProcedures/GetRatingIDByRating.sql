CREATE PROCEDURE [dbo].[GetRatingIDByRating]
	@RatingDescription NVARCHAR(50),
	@RatingID TINYINT OUTPUT
AS
SET NOCOUNT ON 

SELECT @RatingID = RatingID FROM dbo.Rating WHERE RatingDescription = @RatingDescription