CREATE TABLE [dbo].[Users]
(
	[UserID] INT NOT NULL IDENTITY(1,1), 
    [UserName] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserID]) 
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_UserName] ON [dbo].[Users] ([UserName])
