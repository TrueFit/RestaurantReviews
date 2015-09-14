CREATE PROCEDURE [dbo].[GetStateIDByStateName]
	@StateName NVARCHAR(50),
	@StateID SMALLINT OUTPUT
AS
SET NOCOUNT ON 

SELECT @StateID = StateID FROM dbo.States WHERE StateName  = @StateName