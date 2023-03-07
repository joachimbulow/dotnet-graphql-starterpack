using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Starterpack.Auth.Api.Inputs;
using Starterpack.Auth.Domain.Models;
using Starterpack.Auth.Domain.Services;
using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Models;

public struct UserAndTokens
{
    public TokensModel tokens;
    public UserModel user;
}
static class AuthUtils
{
    public static async Task<UserAndTokens> createUserWithTokens(this WebApplicationFactory<Program> factory, StateFixture state)
    {
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var services = scope.ServiceProvider;
            var userService = services.GetRequiredService<IUserService>();
            var authService = services.GetRequiredService<IAuthService>();

            var email = state.nextEmail();
            var password = state.nextPassword();
            var userModel = await userService.CreateUserAsync(new CreateUserInput(state.nextName(), email, password), CancellationToken.None);
            var tokensModel = await authService.LoginAsync(new LoginInput(email, password), CancellationToken.None);

            return new UserAndTokens { tokens = tokensModel, user = userModel };
        }
    }
}
