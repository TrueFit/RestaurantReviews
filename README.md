RestaurantReviews
=================

The Problem
--------------
Truefit is in the midst of building a mobile application that will let restaurant patrons rate the restaurant in which they are eating. As part of the build, we need to develop an API that will accept and store the ratings and other sundry data. 

For this project, we would like you to build this api. Feel free to add your own twists and ideas to what type of data we should collect and return, but at a minimum your API should be able to:

1. Get a list of restaurants by city
2. Post a restaurant that is not in the database
3. Post a review for a restaurant
4. Get of a list of reviews by user
5. Delete a review

The Fine Print
--------------
Please use whatever technology and techniques you feel are applicable to solve the problem. We suggest that you approach this exercise as if this code was part of a larger system. The end result should be representative of your abilities and style.

Please fork this repository. When you have completed your solution, please issue a pull request to notify us that you are ready.

Have fun.


The Solution
--------------
This solution is divided up into three projects:

	1. RestaurantReviews.API  - This is the public-facing HTTP Restful API. It is broken up into three separate controllers:
	    a. RestaurantController - handles actions specific to restuarants or actions over data in the context of a particular restaurant.
	    b. ReviewController     - handles actions specific to reviews or actions over data in the context of a particular review.
	    c. UserController       - handles actions specific to users or actions over data in the context of a particular user.

	2. RestaurantReviews.Data - This is the data acess layer which handles all of the interaction with the data storage engine. In this
	   project, I implement data access via Entity Framework. Classes in this project are split into partial classes:
	    - <EntityClass>.cs               - This partial contains the model's properties and EF-specific attributes.
	    - <EntityClass>.StaticMethods.cs - This partial contains static methods used to directly access the database.

	3. RestaurantReviews.DB   - This is the database project used for maintaining the schema for our database as well as (for the future) managing
	                            data migrations.

There are a set of stubbed unit tests for both the .API and .Data projects, however I did not implement these unit tests at this time. If it becomes necessary, they
will be added and this document will be updated to reflect the new projects. Additionally, MSDN-style API documentation has been produced for both the .API and
.Data projects. This was generated via Doxygen (http://www.doxygen.org).

This API (and data layer) are by no means a complete implementation. This code base is to serve the purpose of demonstrating coding and architectural style. Given tighter
specifications, I may have made changes to architecture. One such change might be to not spin up a new connection to the DB each time data needs to be fetched for 
the API. Another might be to create DTO's for moving data between client and server. The model classes used to solve this problem are small enough (and devoid of instance)
logic, so there need not be API-specific model classes, but as the scope and implementation of this API grows, using models might be beneficial to improve throughput.

I did not take caching into consideration as I believe that to be outside the scope of the project given the current requirements, but for any public-facing API where
performance is a concern, caching data - rather than performing a DB fetch on each request - would be beneficial here. 

User administration is virtually nonexistent in this API because it is not the job of a restaurant reviews API to handle user-specific functions. Another API could/should
be developed for that purpose. I've only included the getter and setter as a matter of convenience. In a real world implementation, I would not task this API with any 
user admin.


Addendum
--------------
With some time on my hands, I re-implemented the API (from spec) in JavaScript over Node.JS, Express, and MongoDB. The public-facing API is basically the same, although I didn't mirror the .NET API.

My Node.JS architecture feels "Angular-ish". My application will start up (app.js) create an Express router, and load all of my routes onto that router by dependency injection. Each route is defined in its own js file in the routes folder and again uses dependency injection to specify the callback method for each route. The callback handlers are each defined in controller specific to the type of data it will handle, thusly I have created two controllers: one for handling restaurants and another for handling reviews.

In each of the controllers, I variablize the functions and expose only those which are needed outside of the controller itself. While not present in the current version of any controller, this will allow for the expansion of the controller, without expanding the API unnecessarily (thinking validation, helpers, and private configuration).

I prefer to dissect the architecture this way to split up the implementation of the routing, the data modeling, and the API. There is easier code to read and follow than this, but this is very extensible and limits regressions.