Notes:
--------------
Had an issue with SQLite with EF drivers installing on my machine, so I decided to use a Micro-ORM called Massive (https://github.com/FransBouma/Massive) instead of using ADO.NET.
That ended up being not a very good idea, because Massive is based on dynamically typed objects which are annoying to work with in a strongly typed environment. Should have just went with ADO.

No exception catching, failures dump to output.

There is no validation of data coming from the client, but because in the database the primary key must not be null and it is allowed to be null for autoincremented inserts, a client could pass a null id and get back an undesired result. Also, other unique fields are not checked, which will lead to failed updates and inserts if it constraints are violated. Would have used something like Fluent Validation.

No authentication. Someone could wipe all the records or carpet bomb the database without even trying. There's also no throttling, you can hit the api as much as you want.

Everything is integer-based IDs, would have rather used a unique name instead in retrospect.

Code Layout:
--------------
RstrntAPI.DataAccess - data access layer, contains Massive ORM and entities.
RstrntAPI.DTO - data transfer objects used to pass information between layers.
RstrntAPI.Repository - Uses DataAccess to fill DTOs
RstrntAPI - API, uses DTOs as response models, not exactly how it's supposed to be used but it works with a small project like this.

Did not implement a business layer, it would have been nearly identical to Repository since there is no logging, auditing, or any logic taking place.

Usage:
--------------
Accepted Verbs:
GET for fetching
PUT for updating
POST for inserting
DELETE for deleting

Getting a list of restaurants:
http://localhost/api/v1/Restaurants

Getting information about a restaurant and all of it's branches:
http://localhost/api/v1/Restaurants/{restaurantId}

Getting detailed info about a restaurant and it's branches:
http://localhost/api/v1/Restaurants/{restaurantId}/Detail

Getting detailed info about a restaurant branch:
http://localhost/api/v1/Restaurants/{restaurantId}/Location/{locationId}

Getting detailed info about restaurant branch within a city:
http://localhost/api/v1/Restaurants/{restaurantId}/City/{cityId}

Searching:
http://localhost/api/v1/Search/(Restaurants|Reviews|Users|City)?term=searchterm

User and User's Reviews:
http://localhost/api/v1/User/{userid}
http://localhost/api/v1/User/{userid}/Reviews

How to insert a new restaurant in a new city:
--------------
1. Insert new City -- if city exists, look up ID instead
POST - http://localhost/api/v1/City with the body being in json format:
{ name: 'CityName' }
You will get a json returned with the name and id if successful.

2. Insert new Restaurant -- if restaurant exists and you are just adding a branch, skip this step and lookup the restaurant id instead.
POST - http://localhost/api/v1/Restaurant with the body being in json format:
{ name: 'RestaurantName' }
You will get a json returned with the name and id if successful.

3. Insert new Location -- This is basically a branch office, 1-m relationship to restaurant.
POST - http://localhost/api/v1/Location with the body being in json format:
{ 
	CityId: id from above,
	RestaurantId: id from above,
	StreetAddress: 'Address of branch location'
}
You will get a json returned with the assigned id if successful.


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