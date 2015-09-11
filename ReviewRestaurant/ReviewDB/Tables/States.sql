CREATE TABLE [dbo].[States]
(
	[StateID] SMALLINT IDENTITY(1,1) , 
    [StateName] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_States] PRIMARY KEY ([StateID])
)
