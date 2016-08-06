Notes:
--------------
```
Had an issue with SQLite with EF drivers installing on my machine, so I decided to use a Micro-ORM called Massive (https://github.com/FransBouma/Massive) instead of using ADO.NET.
That ended up being not a very good idea; Massive is dynamically typed and not quite as mature as I hoped. 
Would have been better to use ADO or EF6. 
The current implementation is very basic and not efficient, for example many of the database request pull back all table data.

Validation doesn't check for constraints on the database.

No authentication. Someone could wipe all the records or carpet bomb the database without even trying. There's also no throttling, you can hit the api as much as you want.

A document store would have been nice, instead of the relational database. I opted not to do that because I wanted this project to run without prior configuration.
```

Assumptions:
--------------
Assumes Content-Type is application/json

City is as broad as it gets, no sense of state, country, zip code, etc. Cities with the same name are possible, but not recommended.

Two of the same restaurant name is possible, again not recommended.

AccountName (username) is unique. No checking for this constraint is done, it will just response as a failure if you try to create it more than once.

There is a lot of stuff that I feel is outside the scope of this project, so I didn't implement it. Such as authentication, better exception handling, much better data access and database design, async tasks, parallel foreach loops, etc.

Usage:
--------------
```
Accepted Verbs:
GET for fetching
PUT for updating
POST for inserting
DELETE for deleting

For create, omit "Id" from the request model, as it is autoincremented.

Restaurants
GET api/v1/Restaurant/
GET api/v1/Restaurant/{RestaurantId}/Detail
GET api/v1/Restaurant/{RestaurantId}/Location/{LocationId}
GET api/v1/Restaurant/{RestaurantId}/City/{CityId}
GET api/v1/Restaurant/City/{CityId}
POST api/v1/Restaurant/
DELETE api/v1/Restaurant/{RestaurantId}
PUT api/v1/Restaurant/

Request Model:
{
	Id: int,
	Name: string[1024]
}

City
GET api/v1/City/
GET api/v1/City/Restaurant/{RestaurantId}
GET api/v1/City/{CityId}
POST api/v1/City
DELETE api/v1/City/{CityId}
PUT api/v1/City

Request Model:
{
	Id: int,
	Name: string[1024]
}

Location
GET api/v1/Location/
GET api/v1/Location/City/{CityId}
GET api/v1/Location/Restaurant/{RestaurantId}
GET api/v1/Location/{LocationId}
POST api/v1/Location
DELETE api/v1/Location/{LocationId}
PUT api/v1/Location

Request Model:
{
	Id: int,
	CityId: int,
	RestaurantId: int,
	StreetAddress: string[1024]
}

Reviews
GET api/v1/Reviews/
GET api/v1/Reviews/{ReviewsId}
POST api/v1/Reviews
DELETE api/v1/Reviews/{ReviewsId}
PUT api/v1/Reviews

Request Model:
{
	Id: int,
	LocationId: int,
	UserId: int,
	Subject: string[1024],
	Body: string[4096]
}

User
GET api/v1/User
GET api/v1/User/{ReviewId}
POST api/v1/User
DELETE api/v1/User/{ReviewsId}
PUT api/v1/Reviews

Request Model:
{
	Id: int,
	AccountName: string[1024],
	FullName: string[1024],
	Hometown: int:CityId
}

Search
GET api/v1/Search/Restaurants?term=
GET api/v1/Search/Reviews?term=
GET api/v1/Search/Users?term=
GET api/v1/Search/City?term=


Response Model for all requests except restaurant detail/loc/city and user reviews:
{
	NameOfObject: array of type of Request Model
	HasErrors: boolean,
	ErrorMessages: array of string, or null if no errors
}
Response Model for user reviews
{
	User: array of User request model,
	Reviews: array of Review request model,
	HasErrors: boolean,
	ErrorMessages: array of string, or null if no errors
}
Response Model for restaurant detail/, location/, and city/ requests
{
	Restaurants: array of locations
	[
		{
			RestaurantId,
			LocationId,
			CityId,
			RestaurantName,
			StreetAddress,
			City
			Reviews: []
		}
	]
	HasErrors: boolean,
	ErrorMessages: array of string, or null if no errors
}
```

How to insert a new restaurant in a new city:
--------------
```
1. Insert new City -- if city exists, look up ID instead
POST - http://localhost/api/v1/City
Request model:
{ 
	name: 'CityName' 
}

2. Insert new Restaurant -- if restaurant exists and you are just adding a branch, skip this step and lookup the restaurant id instead.
POST - http://localhost/api/v1/Restaurant
Request model:
{ 
	name: 'RestaurantName' 
}

3. Insert new Location -- This is basically a branch office, 1-m relationship to restaurant.
POST - http://localhost/api/v1/Location
Request model:
{ 
	CityId: id from above,
	RestaurantId: id from above,
	StreetAddress: 'Address of branch location'
}
```

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
