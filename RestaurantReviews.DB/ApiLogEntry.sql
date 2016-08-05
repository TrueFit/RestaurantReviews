CREATE TABLE [dbo].[ApiLogEntry]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Application] VARCHAR(50) NULL, 
    [User] VARCHAR(50) NULL, 
    [Machine] VARCHAR(50) NULL, 
    [RequestIPAddress] VARCHAR(20) NULL, 
    [RequestContentType] VARCHAR(200) NULL, 
    [RequestContentBody] VARCHAR(MAX) NULL, 
    [RequestUri] VARCHAR(255) NULL, 
    [RequestMethod] VARCHAR(10) NULL, 
    [RequestRouteTemplate] VARCHAR(500) NULL, 
    [RequestRouteData] VARCHAR(500) NULL, 
    [RequestHeaders] VARCHAR(500) NULL, 
    [RequestTimeStamp] DATETIME NULL DEFAULT (getdate()), 
    [ResponseContentType] VARCHAR(200) NULL, 
    [ResponseContentBody] VARCHAR(MAX) NULL, 
    [ResponseStatusCode] INT NULL, 
    [ResponseHeaders] VARCHAR(500) NULL, 
    [ResponseTimeStamp] DATETIME NULL DEFAULT (getdate())
)
