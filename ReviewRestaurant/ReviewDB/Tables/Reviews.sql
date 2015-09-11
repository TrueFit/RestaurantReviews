CREATE TABLE [dbo].[Reviews]
(
	[ReviewID] INT NOT NULL IDENTITY(1,1), 
    [UserID] INT NOT NULL, 
    [RestaurantID] INT NOT NULL, 
    [RatingID] TINYINT NOT NULL, 
    [ReviewText] NVARCHAR(1000) NOT NULL, 
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([ReviewID]), 
    CONSTRAINT [FK_Reviews_Users] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID]), 
    CONSTRAINT [FK_Reviews_Restaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [Restaurant]([RestaurantID]), 
    CONSTRAINT [FK_Reviews_Rating] FOREIGN KEY ([RatingID]) REFERENCES [Rating]([RatingID])
)
