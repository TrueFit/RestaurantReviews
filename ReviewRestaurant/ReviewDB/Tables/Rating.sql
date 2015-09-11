CREATE TABLE [dbo].[Rating]
(
	[RatingID] TINYINT NOT NULL IDENTITY(1,1), 
    [RatingDescription] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Rating] PRIMARY KEY ([RatingID])
)
