CREATE PROCEDURE [dbo].[GetReviewByID]
	@ReviewID INT
AS
SET NOCOUNT ON

SELECT rv.ReviewID,
u.UserName,
r.RestaurantName,
rt.RatingDescription,
rv.ReviewText,
c.CityName,
s.StateName,
rv.ReviewDate
FROM Reviews rv
INNER JOIN Restaurant r ON r.RestaurantID = rv.RestaurantID
INNER JOIN Rating rt ON rt.RatingID = rv.RatingID
INNER JOIN Cities c ON c.CityID = r.CityID
INNER JOIN States s ON s.StateID = c.StateID
INNER JOIN Users u ON u.UserID = rv.UserID
WHERE rv.ReviewID = @ReviewID

