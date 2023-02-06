# RestaurantReviews
## Introduction
RESTful API that collects and manages reviews for restaurants. Provides basic CRUD operations for users, restaurants, and reviews with validation on reviews. Only users who have not been suspended can contribute to reviews.

## Getting Started
Current version uses in-memory database. When permentant storage is determined update Program.cs with connection string.

Database source controlling has not been implemented. Several options are available based on hosting environment and deploy pipelines.

Common options:
- [SQL Database Projects](https://www.c-sharpcorner.com/article/how-to-create-sql-server-database-project-with-visual-studio/)
- [Entity Code First](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database)
- [DbUp](https://dbup.readthedocs.io/en/latest/)

## Build and Test
Clone the repository locally and run to make/test changes. Current version uses in-memory database. 

## Contribute
API was modeled on [Domain-Driven Design](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice). Unit tests have been added to help document and preserve funcitonality as updates are made.