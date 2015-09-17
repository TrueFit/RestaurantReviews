CREATE PROCEDURE [dbo].[GetUserIDByName]
	@Username NVARCHAR(50),
	@UserID INT OUTPUT
AS
SET NOCOUNT ON 

SELECT @UserID = UserID FROM dbo.Users WHERE UserName = @Username