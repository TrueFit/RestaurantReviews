CREATE TABLE [dbo].[Cities]
(
	[CityID] SMALLINT NOT NULL IDENTITY(1,1), 
    [CityName] NVARCHAR(50) NOT NULL, 
    [StateID] SMALLINT NOT NULL, 
    CONSTRAINT [PK_Cities] PRIMARY KEY ([CityID]), 
    CONSTRAINT [FK_Cities_States] FOREIGN KEY ([StateID]) REFERENCES [States]([StateID])
)
