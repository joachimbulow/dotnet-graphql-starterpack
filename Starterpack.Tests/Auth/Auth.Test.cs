using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Abstractions;

using Microsoft.AspNetCore.Mvc.Testing;
using Starterpack.Auth.Domain.Exceptions;

[Collection("State collection")]
public class AuthTest
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly GraphQLHttpClient _client;
    private readonly StateFixture _stateFixture;

    public AuthTest(WebApplicationFactory<Program> factory, StateFixture stateFixture)
    {
        _factory = factory;
        _stateFixture = stateFixture;
        var client = _factory.CreateClient();
        _client = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new Uri("http://localhost/graphql") }, new NewtonsoftJsonSerializer(), client);
    }

    [Fact]
    public async Task LoginWithValidCredentialGivesValidTokens()
    {
        var user = await AuthUtils.createUserWithTokens(_factory, _stateFixture);

        // Send login with correct credentials
        var loginRequest = new GraphQLRequest(LoginMutation,
        variables: new { input = new { email = user.user.Email, password = StateFixture.CommonPassword } });

        var loginResponse = await _client.SendMutationAsync(loginRequest, () => new { login = new LoginPayload() });

        // Assert
        Assert.NotNull(loginResponse.Data.login.TokensModel.AccessToken);
        Assert.NotNull(loginResponse.Data.login.TokensModel.RefreshToken);
    }

    [Fact]
    public async Task LoginWithInvalidCredentialsThrowsInvalidLoginError()
    {
        var user = await AuthUtils.createUserWithTokens(_factory, _stateFixture);

        // Send login with invalid credentials
        var loginRequest = new GraphQLRequest(LoginMutation,
        variables: new { input = new { email = user.user.Email, password = "Some_wrong_password" } });

        var loginResponse = await _client.SendMutationAsync(loginRequest, () => new { login = new LoginPayload() });

        AssertionUtils.AssertExceptionError(loginResponse.Data.login, new InvalidLoginException());
    }

    [Fact]
    public async Task LoginWithInvalidEmailThrowsValidationError()
    {
        var user = await AuthUtils.createUserWithTokens(_factory, _stateFixture);

        // Send login with invalid credentials
        var loginRequest = new GraphQLRequest(LoginMutation,
        variables: new { input = new { email = user.user.Email + "@", password = "Some_wrong_password" } });

        var loginResponse = await _client.SendMutationAsync(loginRequest, () => new { login = new LoginPayload() });

        AssertionUtils.AssertValidationError(loginResponse.Data.login, "Email");
    }



    private string LoginMutation = @"
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
    }";
}