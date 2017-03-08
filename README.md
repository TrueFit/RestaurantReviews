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

Submission Notes
--------------
API calls:
1. Get a list of restaurants by city
GET /restaurants?city=[string of city name]
e.g. /restaurants?city=Pittsburgh -> returns the restaurants listed as being in Pittsburgh

2. Post a restaurant that is not in the database
POST /restaurants
example payload: {"restaurant":{"name":"Eleven","location":"Pittsburgh"}}

3. Post a review for a restaurant
POST /reviews
example payload: {"review":{"title":"Eleven rules!","body":"An all-around great dining experience!","rating":"10","user_id":"1","restaurant_id":"4"}}

4. Get of a list of reviews by user
GET /reviews?user_id=[ID of user]
e.g. /reviews?user_id=1 -> returns the reviews from the user with ID of 1

5. Delete a review
DELETE /reviews/[ID of review]
e.g. DELETE /reviews/1 -> deletes the review with ID of 1

Authentication:
Users must get a token with a call to /token, passing in the base64 encoded string of their email:password. This token then needs to be used for all API calls, passed in with the Authorization header.

Future Considerations
--------------
1. This initial version simply uses the city names stored as strings to denote the locations of restaurants - future versions could elaborate on this by allowing more complex location input (e.g. partial/full street addresses, lat/lon coordinates, etc.).
2. It might be beneficial to build in multiple types of users, for instance if you wanted to give different capabilities for restaurant owners, or have different levels of administrators/moderators to edit content.
3. It would be good to build in a versioning system for this API, to be agreed upon with the client-side developers. This would help safeguard for any future changes/additions.
4. Depending on the UI/UX of the application, pagination could be added to the calls to better deal with long lists of data.
