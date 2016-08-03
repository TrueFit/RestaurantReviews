CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] VARCHAR(20) NOT NULL, 
    [Locked] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [AK_User_Username] UNIQUE (Username) 
)
