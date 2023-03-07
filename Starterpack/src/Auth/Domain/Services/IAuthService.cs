using Starterpack.Auth.Api.Inputs;
using Starterpack.Auth.Domain.Models;

namespace Starterpack.Auth.Domain.Services
{
    public interface IAuthService
    {
        Task<TokensModel> LoginAsync(LoginInput loginInput, CancellationToken cancellationToken);

        Task<TokensModel> RefreshTokensAsync(RefreshTokenInput refreshTokenInput, CancellationToken cancellationToken);
    }
}