CREATE PROCEDURE [dbo].[GetRestaurantIDByRestaurantName]
	@RestaurantName NVARCHAR(50),
	@RestaurantID INT OUTPUT
AS
SET NOCOUNT ON 

SELECT @RestaurantID = RestaurantID FROM dbo.Restaurant WHERE RestaurantName = @RestaurantName