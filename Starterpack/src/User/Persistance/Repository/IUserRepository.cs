// generate the interface for UserRepository.cs

using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Models;
using Spazw.User.Api.Inputs;

public interface IUserRepository
{
    Task<UserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken);

    Task<UserModel> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken);

    Task<UserModel> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken);
}