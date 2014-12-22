Running the API
===

The API server is written using Node.js. To run the code, install the latest version of Node.js, and run the following commands:

```
npm install       <-- this will install all necessary packages.
npm run db:seed   <-- this will create the SQLite database at db/development.sqlite and seed the database with sample data
node server.js    <-- start the API server
```

The API will be available at http://localhost:3000.  To get started, try some of the following routes:

  ```
  /restaurants                    <-- see all of the restaurants in the system
  /users                          <-- see all of the users in the system
  /restaurants?city=pittsburgh    <-- get all of the restaurants in Pittsburgh
  /users/ntesla/reviews           <-- get all reviews left by Nicola Tesla
  /restaurants/brgr/reviews       <-- get all reviews for the BRGR restaurant
  ```


API Reference
====

Standard HTTP response codes are used.  A few common codes that are returned by the API are shown below. For more, see resources/api.js:

  ```
  200 OK          Succesful Request
  201 Created     A new resource was created successfully
  404 Not Found   Unable to find the resource requested
  ```

Restaurants
---

**GET RESTAURANTS**

  ```
  Route: /restaurants
  Query String: city
  Method: GET
  ```

  Returns a full list of all the restaurants in the system, as a JSON array. If the query string contains a city filter, only restaurants in that city are returned. And example request and response follows:

  **REQUEST:**
  ```
  GET /restaurants?city=pittsburgh HTTP/1.1
  Host: localhost:3000
  Cache-Control: no-cache
  ```

  **RESPONSE:**
  ```
  [
    {
      "id": 1,
      "name": "Cure",
      "city": "Pittsburgh",
      "state": "PA",
      "slug": "cure",
      "createdAt": "2014-12-22T06:37:39.000Z",
      "updatedAt": "2014-12-22T06:37:39.000Z"
    },
    {
      "id": 3,
      "name": "Butcher and the Rye",
      "city": "Pittsburgh",
      "state": "PA",
      "slug": "butcher-and-the-rye",
      "createdAt": "2014-12-22T06:37:39.000Z",
      "updatedAt": "2014-12-22T06:37:39.000Z"
    }
  ]
  ```

**GET RESTAURANTS BY SLUG**

  ```
  Route: /restaurants/[slug]
  Method: GET
  ```

  Returns the restaurant matching the specified slug. For example, a request to /restaurants/butcher-and-the-rye, returns details for the restaurant named "Butcher and the Rye"


**CREATE A NEW RESTAURANT**

  ```
  Route: /restaurants/
  Method: POST
  ```

  Creates a new restaurant. The request should include the content-type header of application/json. The body of the request, should be in JSON format. An example request follows:

  ```
  POST /restaurants/ HTTP/1.1
  Host: localhost:3000
  content-type: application/json
  Cache-Control: no-cache

  {
    "name":"Christy Hill",
    "city":"Tahoe City",
    "state":"CA",
    "slug":"christy-hill"
  }
  ```

  If the request (and creation of the restaurant is successful, the API will respond with 201 Created and will return the record corresponding to the newly created restaurant.

**GET REVIEWS FOR A RESTAURANT**

  ```
  Route: /restaurants/[slug]/reviews
  Method: GET
  ```

  Returns a restaurants details, along with all reviews for the restaurant. An example request and response follow:

  **REQUEST:**
  ```
  GET /restaurants/brgr/reviews HTTP/1.1
  Host: localhost:3000
  Cache-Control: no-cache
  ```

  **RESPONSE:**
  ```
  {
    "id": 2,
    "name": "BRGR",
    "city": "Cranberry",
    "state": "PA",
    "slug": "brgr",
    "createdAt": "2014-12-22T05:30:54.000Z",
    "updatedAt": "2014-12-22T05:30:54.000Z",
    "Reviews": [
      {
        "id": 3,
        "content": "It was alright. Will go back, but hopefully the service is better next time.",
        "num_stars": 3,
        "createdAt": "2014-12-22T05:30:54.000Z",
        "updatedAt": "2014-12-22T05:30:54.000Z",
        "restaurant_id": 2,
        "user_id": 2
      },
      {
        "id": 6,
        "content": "Mmmm..best burger I've ever had. And the milkshakes are good too.",
        "num_stars": 4,
        "createdAt": "2014-12-22T05:30:54.000Z",
        "updatedAt": "2014-12-22T05:30:54.000Z",
        "restaurant_id": 2,
        "user_id": 3
      }
    ]
  }
  ```

**CREATE NEW REVIEW FOR A RESTAURANT**

  ```
  Route: /restaurants/[slug]/reviews
  Method: POST
  ```

  Create a new review associated with a restaurant identified by the slug. An example request follows.  If the review is created successfully, the api will respond with 201 Created and the newly created review, otherwise it will respond with 400 Bad Request.

  **REQUEST:**
  ```
  POST /restaurants/christy-hill/reviews HTTP/1.1
  Host: localhost:3000
  content-type: application/json
  Cache-Control: no-cache

  {
    "user_id": 1,
    "review": {
      "content": "Really awesome place right on the shores of Lake Tahoe. Can't beat the view, and in the winter its easy to get a table",
      "num_stars": 4
    }
  }
  ```

Reviews
===

**DELETE A RESTAURANT REVIEW**

  ```
  Route: /reviews/[id]
  Method: DELETE
  ```

  To delete a review, issue a DELETE request with the reviews ID. If deleted successfully, the API will respond with 204 No Content.


Users
===

**GET A LIST OF USERS**

  ```
  Route: /users
  Method: GET
  ```

  Returns a listing of all users in the system. Obviously, this is not a route we would include in a real-world application.

**GET AN INDIVIDUAL USERS' DETAILS**

  ```
  Route: /users/[id|name]
  Method: GET
  ```

  Return a users' details, looking up by the users' database ID, or a users' username (i.e. aeinstein, ntesla)


**GET ALL OF THE REVIEWS FOR A GIVEN USER**

  ```
  Route: /users/[id|name]/reviews
  Method: GET
  ```

  Returns all of the reviews for a given user, specified by user ID or username



Original Exercise
===

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
