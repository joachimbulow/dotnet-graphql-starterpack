using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

public static class GraphqlClientExtensions
{
    public static void AuthenticateClient(this GraphQLHttpClient client, UserAndTokens userAndTokens)
    {
        client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAndTokens.tokens.AccessToken);
    }

    public static void UnauthenticateClient(this GraphQLHttpClient client, UserAndTokens userAndTokens)
    {
        client.HttpClient.DefaultRequestHeaders.Authorization = null;
    }

}
