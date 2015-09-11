CREATE PROCEDURE [dbo].[GetAllReviewsByRestaurant]
	@RestaurantName NVARCHAR(50)
AS
SET NOCOUNT ON 

DECLARE @RestaurantID INT

EXEC	[dbo].[GetRestaurantIDByRestaurantName]
		@RestaurantName = @RestaurantName,
		@RestaurantID = @RestaurantID OUTPUT

SELECT r.RestaurantName,
u.Username,
rv.ReviewText,
rt.RatingDescription,
c.CityName,
s.StateName
FROM Restaurant r
INNER JOIN Reviews rv ON rv.RestaurantID = r.RestaurantID
INNER JOIN Rating rt ON rv.RatingID = rt.RatingID
INNER JOIN Users u ON rv.UserID = u.UserID
INNER JOIN Cities c ON c.CityID = r.CityID
INNER JOIN States s ON s.StateID = c.StateID
WHERE r.RestaurantID = @RestaurantID
