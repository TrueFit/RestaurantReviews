/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE ReviewDB
GO

SET IDENTITY_INSERT dbo.Rating ON

INSERT INTO dbo.Rating (RatingID, RatingDescription)
VALUES(1, N'Terrible'),
(2, N'Poor'),
(3, N'Average'),
(4, N'Very Good'),
(5, N'Excellent')

SET IDENTITY_INSERT dbo.Rating OFF

SET IDENTITY_INSERT dbo.States ON

INSERT INTO dbo.States (StateID, StateName)
VALUES(1, N'Pennsylvania'),
(2, N'Ohio')

SET IDENTITY_INSERT dbo.States OFF

SET IDENTITY_INSERT dbo.Cities ON

INSERT dbo.Cities (CityID, CityName, StateID)
VALUES(1, N'Pittsburgh', 1),
(2, 'Penn Hills', 1),
(3, 'Sharon', 1),
(4, 'Boardman', 2)

SET IDENTITY_INSERT dbo.Cities OFF

SET IDENTITY_INSERT dbo.Users ON

INSERT INTO dbo.Users(UserID, UserName)
VALUES(1, N'MissPiggy'),
(2, 'KermitTheFrog'),
(3, 'Gonzo')

SET IDENTITY_INSERT dbo.Users OFF

SET IDENTITY_INSERT dbo.Restaurant ON

INSERT INTO dbo.Restaurant(RestaurantID, RestaurantName, CityID) 
VALUES(1, N'Nakama', 1),
(2, N'Fat Head''s', 1),
(3, N'Tsuki', 2),
(4, N'Vintage Estates', 4),
(5, N'Quaker Steak and Lube', 3)

SET IDENTITY_INSERT dbo.Restaurant OFF


INSERT INTO dbo.Reviews(UserID, RestaurantID, RatingID, ReviewText, ReviewDate) 
VALUES(1, 1, 1, N'This place is terrible!', GETUTCDATE()),
(2, 1, 3, N'This place is ok', GETUTCDATE()),
(3, 1, 5, N'This place is amazing!', GETUTCDATE()),
(1, 3, 4, N'Great sushi!', GETUTCDATE()),
(2, 3, 2, N'Awful sushi.', GETUTCDATE()),
(3, 3, 3, N'Meh.', GETUTCDATE())

