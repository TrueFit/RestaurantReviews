CREATE PROCEDURE [dbo].[GetAllReviewsByCity]
	@CityName NVARCHAR(50)
AS
SET NOCOUNT ON

DECLARE @CityID SMALLINT

EXEC	[dbo].[GetCityIDByCityName]
		@CityName = @CityName,
		@CityID = @CityID OUTPUT


SELECT rv.ReviewID,
r.RestaurantName,
u.Username,
rv.ReviewText,
rt.RatingDescription,
c.CityName,
s.StateName
FROM Cities c
INNER JOIN States s ON c.StateID = s.StateID
INNER JOIN Restaurant r ON r.CityID = c.CityID
INNER JOIN Reviews rv ON rv.RestaurantID = r.RestaurantID
INNER JOIN Users u ON rv.UserID = u.UserID
INNER JOIN Rating rt ON rt.RatingID = rv.RatingID
WHERE c.CityID = @CityID

