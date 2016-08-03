CREATE TABLE [dbo].[APIRequest]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccessDateTime] DATETIME NOT NULL DEFAULT (getdate()), 
    [UserID] INT NOT NULL, 
    [Path] VARCHAR(255) NOT NULL, 
    [PayloadDetails] VARCHAR(1024) NOT NULL
)
