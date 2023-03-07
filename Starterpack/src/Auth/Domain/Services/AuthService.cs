namespace Starterpack.Auth.Domain.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Starterpack.Auth.Api.Inputs;
    using Starterpack.Auth.Domain.Exceptions;
    using Starterpack.Auth.Domain.Models;
    using Starterpack.Auth.Persistence;
    using Starterpack.User.Domain.Models;

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<TokensModel> LoginAsync(LoginInput loginInput, CancellationToken cancellationToken)
        {
            UserModel? loggedInUser = await _authRepository.ValidateLogin(loginInput, cancellationToken);

            if (loggedInUser == null)
            {
                throw new InvalidLoginException();
            }

            return await CreateTokens(loggedInUser, cancellationToken);
        }

        public async Task<TokensModel> RefreshTokensAsync(RefreshTokenInput refreshTokenInput, CancellationToken cancellationToken)
        {
            var user = await _authRepository.ValidateRefreshToken(refreshTokenInput.RefreshToken, cancellationToken);
            return await CreateTokens(user, cancellationToken);
        }

        private async Task<TokensModel> CreateTokens(UserModel user, CancellationToken cancellationToken)
        {
            var accessToken = CreateJwtAccessToken(user);
            var refreshToken = CreateJwtRefreshToken();

            await _authRepository.SaveRefreshToken(refreshToken, user, cancellationToken);

            return new TokensModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string CreateJwtAccessToken(UserModel user)
        {
            var secret = _configuration.GetValue<string>("Authentication:Secret")!;

            // Define the security key and signing credentials for the JWT token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims for the JWT token
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: "Starterpack",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Create a cryptographically secure random refresh token
        private string CreateJwtRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}