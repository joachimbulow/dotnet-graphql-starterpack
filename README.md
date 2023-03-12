# dotnet-graphql-starterpack

This project can be used as a nice starting point for anyone wanting to implement a GraphQL server in Dotnet.

The project scaffolds pretty much every major feature you would need to get started with GraphQL in Dotnet.

Features:

- [x] GraphQL web server using Hot Chocolate 13
- [x] Authentication using Json Web Tokens
- [x] Simple users / account setup for building upon
- [x] Clean example folder structure with SoC
- [x] Working integration tests project included, uncliding SuT and dependency injection and state sharing example
- [x] Scalable query errors
- [x] Scalable and by-the-book graphql validation and exception errors for mutations
- [x] Fluent validation
- [x] Stylecop linting
- [x] Scoped transient database connection using connection pooling (Default is postgres, but you can switch driver)
- [x] Async middleware example, for lazy fetching e.g. the current user in a request
- [x] Example environment variables
- [x] Resolve field example (vague in comment, see documentation, it's not hard)
- [x] AutoMapper setup
- [x] GraphQL voyager setup
- [ ] Data loader example
- [ ] Internationalization example

## Documentation

### Introduction

This project was developed using `dotnet version 7.0.1` .
It proposes a starting point for a `Hot Chocolate 13 GraphQL` server using the power of `asp-net core`.
In these chpaters i will glance over what

###

General overview of backend programming domain
![alt text](https://github.com/joachimbulow/dotnet-graphql-starterpack/domain.png?raw=true)

### Authentication

JWT defines user identity and access validity through private/public key encryption.
Pretty cool, no?

In `program.cs` we intialize using `.AddAuthorization()` in both `ConfigureGraphQLServer` and `RegisterApplicationServices` extension methods.
Furthermore, we sign using a secret using:

    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:Secret")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
        };
    });

Then of course a `RefreshTokenEntity` is generated in `/Auth/Persistence/Entities`. This is used to refresh users' tokens.

Other than, the main auth code is as siple as possible and shoul be easily followed in `/Auth/Services`

### Error handling

#### Query errors

Queries throw errors based on daomin exceptions.
Exceptions should extend the `BaseException` class, and when they are thrown, they will be caught and translated
into a Query Error for the client to consume using the `BaseExceptionMiddleware` defined in `/Common/Domain/Exceptions`.

#### Mutation errors

Mutations also throw errors based on domain exceptions.
However, they are handled differently.
We attach different kinds of errors dynamically. Just add e.g.
`[Error(typeof(RefreshTokenExpiredException))]`

to the mutation method, and the error will be attached to the mutation result if it is thrown.
This is a Hot Chocolate feature, and it is very powerful. Just call `.AddMutationConventions(applyToAllMutations: true)`
in `ConfigureGraphQLServer` in `Program.cs` to enable it.

It can be helpful to visualize it using `graphql` voyager.
Also queries become quite complex. Which is a bit unfortunate.

Your queries will have to look like this to query the errors:

     mutation Login($input: LoginInput!) {
            login(input: $input) {
                tokensModel {
                    accessToken
                    refreshToken
                }
                errors {
    				... on ValidationError {
    				message
    				errors {
    					propertyName
    					errorMessage
    				    }
                    }

    			... on InvalidLoginError {
    				message
    				code
                }
            }
        }
    }

### Integration testing

Integration tests are included, and there is an example of how to access common state,
how to send requests, and a bunch of utils which you can extend on.
