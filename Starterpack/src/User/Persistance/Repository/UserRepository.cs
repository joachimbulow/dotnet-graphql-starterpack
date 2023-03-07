namespace Starterpack.User.Persistance
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Starterpack.Auth.Domain.Services;
    using Starterpack.Auth.Persistance.Entities;
    using Starterpack.Common.Api;
    using Starterpack.User.Api.Inputs;
    using Starterpack.User.Domain.Exceptions;
    using Starterpack.User.Domain.Models;
    using Starterpack.User.Persistance.Entities;
    using Spazw.User.Api.Inputs;

    public class UserRepository : IUserRepository, IAsyncDisposable
    {
        private readonly IMapper _mapper;

        private readonly AppDbContext _dbContext;

        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(IMapper mapper, IDbContextFactory<AppDbContext> dbContextFactory, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _dbContext = dbContextFactory.CreateDbContext();
            _passwordHasher = passwordHasher;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            // Throw custom error if user not found
            UserEntity? user = await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken)
        {
            IEnumerable<UserEntity> users = await _dbContext.Users.ToListAsync();

            return users.Select(u => _mapper.Map<UserModel>(u));
        }

        public async Task<UserModel> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.Users.Where(u => u.Id == input.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            user.Name = input.Name;
            user.Email = input.Email;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken)
        {
            // Make this transactional
            UserEntity? user = _mapper.Map<UserEntity>(input);

            string salt = _passwordHasher.GenerateSalt();

            user.PasswordHash = _passwordHasher.HashPassword(input.Password, salt);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserModel>(user);
        }

        // Cleanup
        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}