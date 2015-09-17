CREATE PROCEDURE [dbo].[AddNewRestaurant]
	@RestaurantName NVARCHAR(50),
	@CityName NVARCHAR(50),
	@RestaurantID INT OUTPUT
AS

SET NOCOUNT ON

DECLARE @CityID INT

EXEC	[dbo].[GetCityIDByCityName]
		@CityName = @CityName,
		@CityID = @CityID OUTPUT

INSERT INTO dbo.Restaurant(RestaurantName, CityID)
VALUES(@RestaurantName, @CityID)

SET @RestaurantID = SCOPE_IDENTITY()