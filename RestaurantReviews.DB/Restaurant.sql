CREATE TABLE [dbo].[Restaurant]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Address1] VARCHAR(100) NOT NULL, 
    [Address2] VARCHAR(100) NOT NULL, 
    [CityID] INT NOT NULL, 
    [ZipCode] VARCHAR(5) NOT NULL, 
    [Phone] VARCHAR(11) NOT NULL, 
    [WebsiteURL] VARCHAR(255) NULL
)
