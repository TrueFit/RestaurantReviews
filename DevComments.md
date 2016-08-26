Developer Comments 
=================
I did not implement any security or membership for the users. I would probably setup OWIN based security and have users log in with their Facebook, Google, or Microsoft accounts since it would be better for users to not have to create another account.

I would usually do much more commenting within the project adding XML comments above methods to outline description and parameters.


Project Structure
-------------- 
RestaurantReviews.Core
This project contains the models used throughout the application. The models are bare so that any data framework could be plugged in as the data layer and not have to modify the models.

RestaurantReviews.Data
This is the data layer. It is where all database querying and transactions are setup. I used EntityFramework to create the database and created repository objects for the models. If you wanted to run the application, you would need to change the connection string within the RRContext class constructor.

RestaurantReviews.Service
This is the business logic layer. All transactions should go through this service layer as it communicates to the data layor above. All service calls return a ServiceCallResult which contains a ValidationErrors enumerable along with an expected return object. Ex. ID for new objects, Enumerables for lists, etc.

RestaurantReviews.API
I chose to use Web API 2 for the communication to the service layer. I did not use any security on here but it could easily be done using the [Authorize] tags and setting up user authentication using tokens and OWIN. I just created a single API controller which has the methods needed to satisfy this code test.

RestaurantReviews.Test
Test project where I tested the Service and API layers.