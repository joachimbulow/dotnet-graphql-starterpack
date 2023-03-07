namespace Starterpack.Auth.Persistence
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Starterpack.Auth.Api.Inputs;
    using Starterpack.Auth.Domain.Exceptions;
    using Starterpack.Auth.Domain.Models;
    using Starterpack.Auth.Domain.Services;
    using Starterpack.Auth.Persistance.Entities;
    using Starterpack.Common.Api;
    using Starterpack.User.Domain.Exceptions;
    using Starterpack.User.Domain.Models;
    using Starterpack.User.Persistance.Entities;

    public class AuthRepository : IAuthRepository, IAsyncDisposable
    {
        private readonly AppDbContext _dbContext;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IMapper _mapper;

        public AuthRepository(IDbContextFactory<AppDbContext> dbContextFactory, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserModel?> ValidateLogin(LoginInput inputModel, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(u => u.Email == inputModel.Email).FirstOrDefaultAsync();
            if (user?.PasswordHash == null || !_passwordHasher.VerifyHashedPassword(inputModel.Password, user.PasswordHash))
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task SaveRefreshToken(string refreshToken, UserModel user, CancellationToken cancellationToken)
        {
            var existingToken = await _dbContext.RefreshTokens.Where(r => r.UserId == user.Id).FirstOrDefaultAsync();

            var newExpiration = DateTime.Now.AddDays(14);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiresAt = newExpiration;
            }
            else
            {
                // TODO: Use environment variable for expiration time
                var token = new RefreshTokenEntity
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    ExpiresAt = DateTime.Now.AddDays(14),
                };
                await _dbContext.RefreshTokens.AddAsync(token, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserModel> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var token = await _dbContext.RefreshTokens.Where(r => r.Token == refreshToken).FirstOrDefaultAsync();

            if (token == null)
            {
                throw new RefreshTokenNotFoundException();
            }

            var now = DateTime.Now;

            if (token.ExpiresAt < now)
            {
                throw new RefreshTokenExpiredException();
            }

            var user = await _dbContext.Users.Where(u => u.Id == token.UserId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<UserModel>(user);
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}