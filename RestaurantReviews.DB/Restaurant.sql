--Table schema script for creating new Restaurant table
CREATE TABLE [dbo].[Restaurant]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Address1] VARCHAR(100) NOT NULL, 
    [Address2] VARCHAR(100) NULL, 
    [City] VARCHAR(50) NOT NULL,
	[State] VARCHAR(2) NOT NULL, 
    [ZipCode] VARCHAR(5) NOT NULL, 
    [Phone] VARCHAR(11) NOT NULL, 
    [WebsiteURL] VARCHAR(255) NULL, 
    CONSTRAINT [AK_Restaurant_Name_Address1_Zip] UNIQUE (Name, Address1, ZipCode)
)
