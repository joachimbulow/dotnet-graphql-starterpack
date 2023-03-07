namespace Starterpack.Auth.Persistence
{
    using System.Threading.Tasks;
    using Starterpack.Auth.Api.Inputs;
    using Starterpack.Auth.Domain.Models;
    using Starterpack.User.Domain.Models;

    public interface IAuthRepository
    {
        Task<UserModel?> ValidateLogin(LoginInput inputModel, CancellationToken cancellationToken);

        Task<UserModel> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken);

        Task SaveRefreshToken(string refreshToken, UserModel user, CancellationToken cancellationToken);
    }
}