CREATE PROCEDURE [dbo].[DeleteReviewByID]
@ReviewID INT,
@UserName NVARCHAR(50),
@DeleteCount INT OUTPUT
AS
SET NOCOUNT ON

DECLARE @UserID INT

EXEC	[dbo].[GetUserIDByName]
		@UserName = @UserName,
		@UserID = @UserID OUTPUT

DELETE FROM dbo.Reviews WHERE ReviewID = @ReviewID AND UserID = @UserID

SET @DeleteCount = @@ROWCOUNT