CREATE PROCEDURE [dbo].[GetCityIDByCityName]
	@CityName NVARCHAR(50),
	@CityID INT OUTPUT
AS
SET NOCOUNT ON 

SELECT @CityID = CityID FROM dbo.Cities WHERE CityName = @CityName