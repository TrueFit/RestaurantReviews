# Getting Started

Thanks for reviewing my submission. I'm going to be putting stuff in here that shouldn't be in version control (keys, client secrets, etc), but because I'm submitting this to you via PR, there's no other way to get it to you. That said, I made some decisions particularly around Authentication that require a little bit of environment setup, which I will explain now.

## Setting up your development environment for AWS access

The client this project uses for communicating with AWS requires access to AWS credentials. On an AWS server, it would "know" where to find these, but you must set them up in your local environment. I've made an Admin user for this purpose with the following credentials:

- aws_secret_access_key: a936UuYGzc9vesc5VZbgkEX0ntIbkPlpdJBirDFH
- aws_access_key_id AKIAWFCJZKZIXAURF3F5

The [AWS Docs](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html) describe this process in more depth, but (to cut to the chase) the easiest options are to:

- 1) Install the [AWS CLI ](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html) and then (in Powershell) run `aws configure set aws_secret_access_key a936UuYGzc9vesc5VZbgkEX0ntIbkPlpdJBirDFH` and `aws configure set aws_access_key_id AKIAWFCJZKZIXAURF3F5`
- 2) Install the [AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/) and enter the credentials above into their respective slots.

## Authentication Workflow

I've chosen to handle authentication through an AWS Cognito User pool. This includes a "Hosted UI" [here](https://norest.auth.us-east-1.amazoncognito.com/login?client_id=49d6scgb3qfou5r2m3lk07fmho&response_type=code&scope=email+openid&redirect_uri=https%3A%2F%2Fdonothing.com%2F) where users can sign in using credentials they create. This UI authenticates directly with Cognito and returns an "authorization code", explained [here](https://aws.amazon.com/blogs/mobile/understanding-amazon-cognito-user-pool-oauth-2-0-grants/). Upon successful authentication, you are redirected to a specified url along with the authorization code in the route query. This is not exactly the proper auth flow for a mobile app, but as that app does not exist, this allows us to simulate the auth flow. The authorization code is then exchanged for a JWT, with which you can authenticate with my Api. I've included an unprotected endpoint (POST @ <base>api/v1/auth) that will make this exchange and return the token. NOTE: This authorization code is single-use. Calling the auth endpoint with the same code more than once will return a 401. The main benefit of this exchange (as opposed to providing the token directly from the Hosted UI) is that the JWT is never exposed to the end-user. All subsequent calls to protected endpoints can be made with the returned JWT. This can be handled via the postman collection or the Swagger UI.

The JWT will include an Id, which will be saved in the User table along with the other user information. This allows us to connect a specific JWT with a given user, thus any protected endpoint can know which user is accessing it. This also allows more direct/easier control of authorization/claims/roles/etc within the api.

In summary, authentication follows these steps:

1. (New User Only) 


## Next Steps

I realize I've left out several elements that would make this work in an enterprise development context. Further work would need done for:

- Logging. I've added logging statements, but they don't go anywhere.
- Hiding secret keys. I've left a lot of them in source control.
- Deployments.
