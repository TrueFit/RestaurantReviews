API Usage
=================
Below is a lost of API endpoints, as well as examples that should work with the checked in JSON data files.

```
GET /cities

Gets a list of all cities. 

Example:
GET /cities
```
```
GET    /cities/{guid}/{type}

Gets a list of entities by type from a city. 

Example: 
GET /cities/c8a4577a-f449-44c1-9f78-ab0ed43c4cf2/restaurant
```
```
POST /entities

Adds an entity.

Notes:
Trusts data submitted too much, also doesn't log the user that submitted it.

Example:  
POST /entities
{
  Guid : "2D3406D2-86CD-4E50-A56A-474C6618AC04",
  CityGuid: "c8a4577a-f449-44c1-9f78-ab0ed43c4cf2",
  Name: "More Pizza",
  Type: "Restaurant"
}
```
```
GET /entities/{guid}

Gets an entity by it's guid

Example:
GET /entities/0374a4c1-57aa-4dcd-b580-944e9a99bc13
```
```
GET /entities/{guid}/reviews

Gets an entity's reviews

Example:
/entities/0374a4c1-57aa-4dcd-b580-944e9a99bc13/reviews
```
```
POST /entities/{guid}/reviews

Adds a review for an entity

Notes: 
Ignores the guid for the review if you submit one and automatically overwrites it. 
Also lacks authentication that the user submitting the review matches the UserGuid 
in the review (aka we are trusting the mobile app, which is a bad idea).

Example:
POST /entities/0374a4c1-57aa-4dcd-b580-944e9a99bc13/reviews
{
  UserGuid : "19685BD6-1B72-4DE9-BCB5-413DAFBA5DD0",
  Title: "Most Awesome Pizza Ever",
  Body: "I ate so much num num num",
  Rating: 5
}
```
```
DELETE /reviews/{guid}?authToken={token}

Deletes a review.

Notes:
Validates the user based on authToken, however user it validates to is 
hard-coded so you can't get rid of the review unless Test User was the 
user that submitted it. This is about as far as validation got before 
I decided it was time to stop adding things to this test.

Example:
DELETE /reviews/e2703d41-948d-4f34-994b-b42d0b34f2f1?authToken=anystring
```
```
GET /users/authenticate?authToken={token}

Authenticates a user based on the authToken.

Notes:
Pass anything in here, this is just so you can lookup the guid of Test User

Example:
GET /users/authenticate?authToken=anystring
```
```
GET /users/{guid}/reviews

Gets reviews for a user

Notes:
This works for any user guid, so I made an API where you can look them 
up for anyone but can only delete them for Test User

Example:
GET /users/19685bd6-1b72-4de9-bcb5-413dafba5dd0/reviews
```

Process for Determining Design
=================
The requirements given are vague in terms of the client's intentions and what their long term goals are, though it's pretty clear what their short term goals are. Our short term goal should be to both design this system to reflect their long term goals and to avoid over-building at this stage to keep costs down.

Questions I'd Have For Product
--------------
* Will this client expand beyond restaurants in the future?
* Is the client looking for massive scalability (aka trying to build the next Yelp), or will their traffic likely be lower (aka a local newspaper wants it to review Pittsburgh restaurants)
* Are they going to hire developers after the initial development to conitinue development, and what level of developer will they be looking to hire first?
* Is this US centric or international

Sample Product Answers (which the implementation is based on)
--------------
* Yes, the client has a vision to expand to many types of businesses based on City location.
* They want scalability, however they don't want it built in right away. However they would like us to build in a way that modifying the system to horizontally scale won't require a rewrite of the entire system.
* They plan on hiring one senior developer, and they would like us to walk the developer through the design and implementation after the project.
* Plans are for US only, possible international but client doesn't want extra effort to manage this scenario.

Thoughts of Design
--------------
* This architecture is decoupled into Services, however they will be implemented in this phase as components. The goal is to provide components that can be turned into web services later when scaling issues arise.
* There is a downside to this strategy, data integrity is now the responsibility of the application rather than the database because we won't be establishing a relational database. This could have implications down the road if the underlying data needs to be utilized in a manner we aren't building towards. The most obvious use case is reporting, however in a high performance system that should be moved to a data warehouse anyways since we don't want live reporting queries in our production environment.
* Even if we aren't aiming for high test coverage, everything should be testable as much as possible.
* The services themselves are going to come across as fairly useless at this stage, which is true given a lot of these operations are simple CRUD calls. This is designed for the theoritical world where more may be happening behind the scenes later, for instance with Reviews the obvious future feature is an automated process to screen reviews for content (curse words, etc), and possibly workflows for manual auditing of reviews. For these reasons the data repositories are being screened inside the services layer.
* You'll notice not all operations are available for each model, for instance you can't Delete a User. This is by design, we're only exposing what the application needs for now. Adding these operations later is easy enough, however if we currently are designing our app to take advantage of them there's no reason to expose them for accidental use.

If This Was a Real Project
--------------
* I'd probably want to start on geolocation and work from there. Identifying where the user is, where a restaurant address is, and recentering on a city seem to be the most critical parts of this app. Once you have that the rest is easy to build.
* We'd need to work out workflows for user submitted restaurants and reviews. Restaurants probably need approval, reviews probably need filtering by curse words and need a reporting process for inappropriate reviews (phase 2?)
* User authentication should be handled early, and we'd likely want to filter at the Web Api layer. We can intermix admin and user api calls on the same controllers that way.
* A rest api can get out of hand when queries get complex, we could explore graphql however that may cost too much in terms of time (maybe an upgrade once it's successful?)

Things That Are Missing or Could Be Different This Submission
--------------
* User authentication is obviously just a stub, and only used in a fairly simple manner when deleting reviews
* No where near enough validation
* Proper error codes and message being kicked back to the mobile app
* Proper data storage (though the json files work pretty well for this)
* API for cities could be cleaner, maybe a url friendly name? May be nicer to lookup with /cities/pittsburgh-pa/restaurant
* Speaking of nice urls, using generic entities that are typed caught me in a trap where my urls suck because they end up singular rather than plural. At one point I was thinking /cities/pittsburgh-pa/by-type/restaraunt then thought I've already sunk a lot of time I shouldn't into this. I need to stop myself before this becomes a complete application.
* It really bugs me people can post reviews without proper authentication, again I could keep going for awhile but I need to stop.
* Did I really just pass a success/failure boolen back from ReviewService.RemoveUserReview? It should at least indicate if the user wasn't authenicated to delete it or if it wasn't there... yeah, I could spend a couple more hours on this but I think the theme of what you're looking for is met here.
* There's a good bit of happy path testing here, I'm certain there are several non-happy path bugs sitting in here somewhere.
* Probably should move posting an entity from /entities to /cities/{guid}/entities. Seems a little more natural, but doesn't really work if you switch to a geo location model rather than direct city mapping. Hmmm...

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
