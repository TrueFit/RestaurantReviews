# RestaurantReviews

## Solution

For my solution entry, I've implemented a simple restaurant review system with a C# backend, utilizing Clean Architecture and the CQRS pattern.
Currently It's just utilizing an in-memory database, but could be swapped out easily for a legit datastore, using the repository pattern.
Global error handling is implemented with the ErrorOr package, which combines some features of OneOf and FluentResults.

### Considerations

To communicate how to use the API, I've decided to go with Swagger, which is intuitive and easy to use, including a playground of sorts to test calls.

In regards to passing development off to any future developers, I've tried to keep the structure simple/modular.
Using CQRS is a nice way to keep the codebase clean and easy to maintain.
It will be as simple as adding a new controller method, new Command or Query with a handler, and wiring everything up!
Of course, good descriptive text in the code is a huge bonus ;)

## To Run Project

- .NET 6 is required
- Navigate to the root of the project (TrueFoodReviews) in Cmd/Terminal and run the following command:
- `dotnet run --project TrueFoodReviews.Api`


-----


## The Problem
Truefit is in the midst of building a mobile application that will let restaurant patrons rate the restaurant in which they are eating. As part of the build, we need to develop an API that will accept and store the ratings and other sundry data. 

For this project, we would like you to build this api. Feel free to add your own twists and ideas to what type of data we should collect and return, but at a minimum your API should be able to:

1. Create a user 
2. Create a restaurant
3. Create a review for a restaurant
4. Get of a list of reviews by user
5. Get of a list of reviews by restaurant
6. Get a list of restaurants by city
7. Delete a review
8. Block a user from posting a review

## The Fine Print
Please use whatever technology and techniques you feel are applicable to solve the problem. We suggest that you approach this exercise as if this code was part of a larger system. The end result should be representative of your abilities and style.

Please fork this repository. When you have completed your solution, please issue a pull request to notify us that you are ready.

Have fun.

# Things To Consider
Here are a couple of thoughts about the domain that could influence your response:

* We are building a mobile application independent of your development, what might be the best way to communicate to other developer how to user your API?
* After you turn your code over for the API, how might you help ensure future developers can feel confident updating it?