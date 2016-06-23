Process for Determining Design
=================
The requirements given are vague in terms of the client's intentions and what their long term goals are, though it's pretty clear what their short term goals are. Our short term goal should be to both design this system to reflect their long term goals and to avoid over-building at this stage to keep costs down.

Questions I'd Have For Product
--------------
* Will this client expand beyond restaurants in the future?
* Is the client looking for massive scalability (aka trying to build the next Yelp), or will their traffic likely be lower (aka a local newspaper wants it to review Pittsburgh restaurants)
* Are they going to hire developers after the initial development to conitinue development, and what level of developer will they be looking to hire first?

Sample Product Answers (which the implementation is based on)
--------------
* Yes, the client has a vision to expand to many types of businesses based on City location.
* They want scalability, however they don't want it built in right away. However they would like us to build in a way that modifying the system to horizontally scale won't require a rewrite of the entire system.
* They plan on hiring one senior developer, and they would like us to walk the developer through the design and implementation after the project.

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
