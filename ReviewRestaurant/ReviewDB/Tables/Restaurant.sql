CREATE TABLE [dbo].[Restaurant]
(
	[RestaurantID] INT NOT NULL IDENTITY(1,1), 
    [RestaurantName] NVARCHAR(50) NOT NULL, 
    [CityID] SMALLINT NOT NULL, 
    CONSTRAINT [PK_Restaurant] PRIMARY KEY ([RestaurantID]), 
    CONSTRAINT [FK_Restaurant_Cities] FOREIGN KEY ([CityID]) REFERENCES [Cities]([CityID])
)
