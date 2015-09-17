CREATE PROCEDURE [dbo].[GetAllReviewsByUser]
	@UserName NVARCHAR(50)
AS
SET NOCOUNT ON

DECLARE @UserID SMALLINT

EXEC	[dbo].[GetUserIDByName]
		@UserName = @UserName,
		@UserID = @UserID OUTPUT


SELECT rv.ReviewID,
r.RestaurantName,
u.Username,
rv.ReviewText,
rt.RatingDescription,
c.CityName,
s.StateName,
rv.ReviewDate
FROM Users u
INNER JOIN Reviews rv ON rv.UserID = u.UserID
INNER JOIN Restaurant r ON r.RestaurantID = rv.RestaurantID
INNER JOIN Rating rt ON rt.RatingID = rv.RatingID
INNER JOIN Cities c ON c.CityID = r.CityID
INNER JOIN States s ON s.StateID = c.CityID
WHERE rv.UserID = @UserID
GO
