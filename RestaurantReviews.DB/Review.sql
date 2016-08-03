CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserID] INT NOT NULL, 
    [RestaurantID] INT NOT NULL, 
    [CreatedDateTime] DATETIME NOT NULL DEFAULT (getdate()), 
    [IsPositive] BIT NOT NULL DEFAULT 1, 
    [Comments] VARCHAR(MAX) NOT NULL
)
