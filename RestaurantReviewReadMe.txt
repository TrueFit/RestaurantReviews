Restaurant Review ReadMe

The API uses C# and a sql database to solve the restaurant review coding exercise.

To deploy the ReviewDB database you will need to point the project to a sql server instance in the connection properties when publishing.

In the DAL project, the connection string to the database will need to be set on line 15 of the DBConnection class.  This can also be set in the app.config
but it is probably easier just to paste it in at the class.

Basic information is seeded via post deployment script.

Stored procedures called by the DAL are used to insert and access information as well as helper procedures which are not called by the DAL.

Both the DAL and the BL projects are called by the test project where I verified that I have working code and fulfill the requirements of the project.

